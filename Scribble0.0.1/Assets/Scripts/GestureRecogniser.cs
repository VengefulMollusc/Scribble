using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureRecogniser : MonoBehaviour {

    [SerializeField]
    public static int resamplePoints = 64;

    [SerializeField]
    public static float rescaleSize = 250f;

    //[SerializeField]
    //private float recognitionThreshold = 0.8f;

    private static float phi = 0.5f * (-1 + Mathf.Sqrt(5));
    private static float theta = 45f;
    private static float thetaDelta = 2f;

    private List<StrokePath> templates;

    private TemplateLoader loader;

    void Start()
    {
        templates = new List<StrokePath>();
        loader = GetComponent<TemplateLoader>();
        if (loader == null) Debug.LogError("No TemplateLoader found");

        LoadTemplates();

        // Test recognition by using a template path
        Recognise(templates[0]);
    }

    private void LoadTemplates()
    {
        templates = loader.LoadTemplates();

        // LOAD TEMPLATES
        if (templates.Count <= 0) Debug.LogError("No gesture templates loaded");
        Debug.Log(templates.Count + " templates loaded");
    }

    /*
     * Compare a given StrokePath against the templates
     */
    public string Recognise(StrokePath _path)
    {
        // test output of recognition points
        string test = "";
        foreach (Vector2 p in _path.Points())
        {
            test += "(" + p.x + ", " + p.y + ") ";
        }
        Debug.Log(test);
        // end

        if (templates.Count <= 0)
        {
            return "no templates";
        }

        float bestDist = Mathf.Infinity;
        StrokePath bestTemp = null;

        foreach (StrokePath t in templates)
        {
            float distance = DistanceAtBestAngle(_path.Points(), t.Points(), -theta, theta, thetaDelta);

            if (distance < bestDist)
            {
                bestDist = distance;
                bestTemp = t;
            }
        }

        float score = 1f - bestDist / 0.5f * Mathf.Sqrt((rescaleSize * rescaleSize) + (rescaleSize * rescaleSize));
        // return both score AND CLOSEST TEMPLATE match

        Debug.Log("Best distance average: " + bestDist);
        Debug.Log("Score: " + score + " Recognition result: " + bestTemp.Name());

        //if (bestTemp == null || score < recognitionThreshold)
        //{
        //    return "no match";
        //}
        return bestTemp.Name();
    }

    private float DistanceAtBestAngle(List<Vector2> _points, List<Vector2> _template, float _thetaA, float _thetaB, float _delta)
    {
        float x1 = phi * _thetaA + (1 - phi) * _thetaB;
        float f1 = DistanceAtAngle(_points, _template, x1);
        float x2 = (1 - phi) * _thetaA + phi * _thetaB;
        float f2 = DistanceAtAngle(_points, _template, x2);

        while (Mathf.Abs(_thetaB - _thetaA) > _delta)
        {
            if (f1 < f2)
            {
                _thetaB = x2;
                x2 = x1;
                f2 = f1;
                x1 = phi * _thetaA + (1 - phi) * _thetaB;
                f1 = DistanceAtAngle(_points, _template, x1);
            } else
            {
                _thetaA = x1;
                x1 = x2;
                f1 = f2;
                x2 = (1 - phi) * _thetaA + phi * _thetaB;
                f2 = DistanceAtAngle(_points, _template, x2);
            }
        }

        return Mathf.Min(f1, f2);
    }

    /*
     * Returns the average distance between points on two paths
     */
    private float DistanceAtAngle(List<Vector2> _points, List<Vector2> _template, float _theta)
    {
        List<Vector2> newPoints = Utilities.RotateBy(_points, _theta);
        float distance = PathDistance(newPoints, _template);
        return distance;
    }

    /*
     * Calculates the average distance between all points in both paths
     * 
     * This is where modifications would work to get differences on a point-by-point basis.
     * Or not? After recognising as circle - compare to perfect circle and get values.
     */
    private float PathDistance(List<Vector2> _pointsA, List<Vector2> _pointsB)
    {
        float distance = 0;

        for (int i = 0; i < _pointsA.Count; i++)
        {
            distance += Vector2.Distance(_pointsA[i], _pointsB[i]);
        }

        return distance / _pointsA.Count;
    }

    /*
     * Returns a list of the point differences for two paths
     * 
     * possibly modify this to instead calculate difference in direction/'normal'?
     * otherwise points could be identical where lines cross rather than line up
     */
    public List<float> PathDifferences(List<Vector2> _pointsA, List<Vector2> _pointsB)
    {
        List<float> diffs = new List<float>();

        for (int i = 0; i < _pointsA.Count; i++)
        {
            float dist = Vector2.Distance(_pointsA[i], _pointsB[i]);
            diffs.Add(dist);
        }

        return diffs;
    }
}
