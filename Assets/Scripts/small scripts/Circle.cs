using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour
{
    public int segments;
    public float xradius;
    public float yradius;
    LineRenderer line;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        line.loop = true;

        CreatePoints();
    }


    void CreatePoints()
    {
        float x;
        float y;
        //float z = 0f;

        float angle = 20f;

        string debugText = "";

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            debugText += "<Point X=\"" + x + "\" Y=\"" + y + "\"\\>\n";

            line.SetPosition(i, new Vector2(x, y));

            angle += (360f / segments);
        }
        
        Debug.Log(debugText);
    }
}