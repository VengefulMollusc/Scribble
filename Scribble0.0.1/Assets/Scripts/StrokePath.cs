using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokePath
{

    private string gestureName;
    private List<Vector2> rawPoints;
    private List<Vector2> points; // converted points
    private List<Vector2> flippedPoints;
    private List<double> vector;
    private float indicativeAngle;

    public StrokePath(string _name, List<Vector2> _rawPoints)
    {
        gestureName = _name;
        rawPoints = _rawPoints;

        SetupPoints(true);// Default flipping points to true
    }

    public StrokePath(string _name, List<Vector2> _rawPoints, bool _flipPoints)
    {
        gestureName = _name;
        rawPoints = _rawPoints;

        SetupPoints(_flipPoints);
    }

    /*
     * Perform initial setup and conversion of points
     */
    private void SetupPoints(bool _flipPoints)
    {
        // calculate converted points
        points = Utilities.ResamplePoints(rawPoints);

        // normalise size and angle for recognition purposes
        indicativeAngle = Utilities.IndicativeAngle(points);
        points = Utilities.RotateBy(points, -indicativeAngle);
        points = Utilities.ScaleTo(points, GestureRecogniser.rescaleSize);
        points = Utilities.TranslateTo(points, Vector2.zero);

        if (_flipPoints)
        {
            // Flip points vertically to eliminate directional differences
            flippedPoints = Utilities.FlipVertical(points);
        }
    }

    public List<Vector2> Points()
    {
        return points;
    }

    public List<Vector2> FlippedPoints()
    {
        return flippedPoints;
    }

    public string Name()
    {
        return gestureName;
    }
}
