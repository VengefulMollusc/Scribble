using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfForbiddance : StaticLine {

    private Vector2 start;
    private Vector2 end;
    private List<Vector2> path;
    private LineRenderer lineR;
    //private List<float> health;

    public void Initialize(Vector2 _point0, Vector2 _point1)
    {
        lineR = GetComponent<LineRenderer>();

        if (lineR == null) Debug.LogError("No LineRenderer attached");

        path = new List<Vector2>();
        start = _point0;
        end = _point1;
        SetupLine();
    }

    /*
     * Creates extra points along the line according to the global point density and assigns health to each one
     */
    private void SetupLine()
    {
        //// create path
        //float length = Vector2.Distance(start, end);
        //int pointCount = (int)(length * GlobalController.pointDensity) - 2; // -2 for start and end positions?

        //if (pointCount < 0)
        //{
        //    // too short
        //    Destroy(gameObject);
        //}

        //Vector2 step = (end - start) * (length / (float)pointCount + 1);

        path.Add(start);

        //Vector2 temp = start;

        //while (pointCount > 0)
        //{
        //    temp += step;
        //    path.Add(temp);
        //}

        path.Add(end);

        // initialise line renderer
        lineR.positionCount = 2;
        lineR.SetPosition(0, start);
        lineR.SetPosition(1, end);

        //// add health to each point
        //float defaultHealth = GlobalController.lineDefaultHealth;
        //health = new List<float>();
        //for (int i = 0; i < health.Count; i++)
        //{
        //    health.Add(defaultHealth);
        //}

    }

    //public override void Damage(Vector2 _location, float _amount)
    //{
    //    // Find closest point on line to damage source
    //    int closestIndex = 0;
    //    float closestDist = Mathf.Infinity;

    //    for (int i = 0; i < path.Count; i++)
    //    {
    //        float dist = Vector2.Distance(path[i], _location);
    //        if (dist < closestDist)
    //        {
    //            closestIndex = i;
    //            closestDist = dist;
    //        }
    //    }

    //    // damage that section
    //    health[closestIndex] -= _amount;
    //}

    //public override void GetHealth()
    //{
    //    throw new NotImplementedException();
    //}

    public override List<Vector2> GetPath()
    {
        return path;
    }
}
