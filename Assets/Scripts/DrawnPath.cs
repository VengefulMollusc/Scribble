using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnPath : MonoBehaviour {

    private List<Vector2> path;
    private LineRenderer lineR;

    /*
     * Initialise - call after spawning prefab
     * 
     * Requires LineRenderer on parent object
     */
    public void Initialize(Vector2 _point0, Vector2 _point1)
    {
        lineR = GetComponent<LineRenderer>();

        if (lineR == null) Debug.LogError("No LineRenderer attached");

        // initialise first two points
        path = new List<Vector2>();
        path.Add(_point0);
        path.Add(_point1);

        // initialise line
        lineR.positionCount = 2;
        lineR.SetPosition(0, path[0]);
        lineR.SetPosition(1, path[1]);
    }

    /*
     * Adds a point to the line - called on mouse move
     */
    public void AddPoint(Vector2 _newPoint)
    {
        path.Add(_newPoint);
        lineR.positionCount = path.Count;
        lineR.SetPosition(path.Count-1, _newPoint);
    }

    /*
     * Returns a StrokePath representation of the drawn path
     */
    public StrokePath GetStrokePath()
    {
        return new StrokePath("drawn", path);
    }
}
