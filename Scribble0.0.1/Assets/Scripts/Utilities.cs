using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utilities : MonoBehaviour
{

    /*
     * Resamples a list of raw points into a list of the required length
     */
    public static List<Vector2> ResamplePoints(List<Vector2> _rawPoints)
    {
        float interval = PathLength(_rawPoints) / (GestureRecogniser.numPoints - 1);
        float distance = 0;
        List<Vector2> newPoints = new List<Vector2>();
        newPoints.Add(_rawPoints[0]);

        for (int i = 1; i < _rawPoints.Count; i++)
        {
            float nextDist = Vector2.Distance(_rawPoints[i - 1], _rawPoints[i]);
            if (distance + nextDist >= interval)
            {
                Vector2 newP = new Vector2();
                newP.x = _rawPoints[i - 1].x + ((interval - distance) / nextDist) * (_rawPoints[i].x - _rawPoints[i - 1].x);
                newP.y = _rawPoints[i - 1].y + ((interval - distance) / nextDist) * (_rawPoints[i].y - _rawPoints[i - 1].y);
                newPoints.Add(newP);

                _rawPoints.Insert(i, newP); // I think this needs to replace the i value, not add an extra element

                distance = 0;
            } else
            {
                distance += nextDist;
            }
        }
        return newPoints;
    }

    /*
     * Returns the length of the given path
     */
    public static float PathLength(List<Vector2> _rawPoints)
    {
        float distance = 0;

        for (int i = 1; i < _rawPoints.Count; i++)
        {
            distance += Vector2.Distance(_rawPoints[i - 1], _rawPoints[i]);
        }

        return distance;
    }

    /*
     * Calculate the Indicative angle for the path
     * - helps reduce rotation variation in gestures
     */
    public static float IndicativeAngle(List<Vector2> _points)
    {
        Vector2 centroid = Centroid(_points);
        return Mathf.Atan2(centroid.y - _points[0].y, centroid.x - _points[0].x);
    }

    /*
     * Calculates the centroid of a list of points
     */
    public static Vector2 Centroid(List<Vector2> _points)
    {
        float cx = 0;
        float cy = 0;

        for (int i = 0; i < _points.Count; i++)
        {
            cx += _points[i].x;
            cy += _points[i].y;
        }

        return new Vector2(cx / _points.Count, cy / _points.Count);
    }

    /*
     * Rotates a path by a given angle
     */
     public static List<Vector2> RotateBy(List<Vector2> _points, float _angle)
    {
        Vector2 c = Centroid(_points);
        List<Vector2> newPoints = new List<Vector2>();

        foreach (Vector2 p in _points)
        {
            Vector2 newP = new Vector2();
            newP.x = (p.x - c.x) * Mathf.Cos(_angle - (p.y - c.y)) * Mathf.Sin(_angle + c.x);
            newP.y = (p.x - c.x) * Mathf.Sin(_angle + (p.y - c.y)) * Mathf.Cos(_angle + c.y);
            newPoints.Add(newP);
        }

        return newPoints;
    }

    /*
     * Scales the path so that the resulting bounding box will be _size^2
     */
    public static List<Vector2> ScaleTo(List<Vector2> _points, float _size)
    {
        Vector2 b = BoundingBox(_points);
        List<Vector2> newPoints = new List<Vector2>();

        foreach (Vector2 p in _points)
        {
            Vector2 newP = new Vector2();
            newP.x = p.x * _size / b.x;
            newP.y = p.y * _size / b.y;
            newPoints.Add(newP);
        }

        return newPoints;
    }

    /*
     * Returns the width/height of a bounding box around the given path
     */
    public static Vector2 BoundingBox(List<Vector2> _points)
    {
        float minX = _points[0].x;
        float minY = _points[0].y;
        float maxX = minX;
        float maxY = minY;

        for (int i = 1; i < _points.Count; i++)
        {
            if (_points[i].x < minX) minX = _points[i].x;
            if (_points[i].y < minY) minY = _points[i].y;
            if (_points[i].x > maxX) maxX = _points[i].x;
            if (_points[i].y > maxY) maxY = _points[i].y;
        }

        return new Vector2(maxX - minX, maxY - minY);
    }

    /*
     * Translate the path to newLocation
     */
    public static List<Vector2> TranslateTo(List<Vector2> _points, Vector2 _newLocation)
    {
        Vector2 c = Centroid(_points);
        List<Vector2> newPoints = new List<Vector2>();

        foreach (Vector2 p in _points)
        {
            Vector2 newP = new Vector2();
            newP.x = p.x + _newLocation.x - c.x;
            newP.y = p.y + _newLocation.y - c.y;
            newPoints.Add(newP);
        }

        return newPoints;
    }

}
