using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokePath
{

    private string gestureName;
    private List<Vector2> rawPoints;
    private List<Vector2> points; // converted points
    private List<double> vector;
    private float indicativeAngle;

    public StrokePath(string _name, List<Vector2> _rawPoints)
    {
        gestureName = _name;
        rawPoints = _rawPoints;

        // calculate converted points
        points = Utilities.ResamplePoints(rawPoints);

        // normalise size and angle for recognition purposes
        indicativeAngle = Utilities.IndicativeAngle(points);
        points = Utilities.RotateBy(points, -indicativeAngle);
        points = Utilities.ScaleTo(points, GestureRecogniser.rescaleSize);
        points = Utilities.TranslateTo(points, Vector2.zero);

        //GetSize();
    }

    //private void GetSize()
    //{
    //    float minX = points[0].x;
    //    float minY = points[0].y;
    //    float maxX = minX;
    //    float maxY = minY;

    //    for (int i = 1; i < points.Count; i++)
    //    {
    //        if (points[i].x < minX) minX = points[i].x;
    //        if (points[i].y < minY) minY = points[i].y;
    //        if (points[i].x > maxX) maxX = points[i].x;
    //        if (points[i].y > maxY) maxY = points[i].y;
    //    }

    //    Debug.Log("min: " + minX + ", " + minY + "  max: " + maxX + ", " + maxY);
    //}

    public List<Vector2> Points()
    {
        return points;
    }

    public string Name()
    {
        return gestureName;
    }
}
