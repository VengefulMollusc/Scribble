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
    }

    public List<Vector2> Points()
    {
        return points;
    }

    public string Name()
    {
        return gestureName;
    }
}
