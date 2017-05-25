using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    private GestureRecogniser gesture;

    [SerializeField]
    private GameObject drawnPathPrefab;
    private DrawnPath currentPath;

    // delete this later
    private DrawnPath tempPath;

    [SerializeField]
    private float mouseMoveThreshold = 0.1f;

    private Vector2 previousMousePos;

    private bool drawing = false;

    void Start()
    {
        if (drawnPathPrefab == null) Debug.LogError("No DrawnPath prefab");

        gesture = GetComponent<GestureRecogniser>();

        if (gesture == null) Debug.LogError("No GestureRecogniser prefab");
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            previousMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (currentPath != null)
            {
                Destroy(currentPath.gameObject);
                Destroy(tempPath.gameObject);
                currentPath = null;
            }
        }

        if (Input.GetMouseButtonUp(0) && drawing)
        {
            StrokePath path = currentPath.GetStrokePath();

            // draws points to test
            // Delete section once proper recognition working
            List<Vector2> points = path.Points();

            for (int i = 0; i < points.Count; i++)
            {
                points[i] = new Vector2(points[i].x + Screen.width / 2, points[i].y + Screen.height / 2);
            }

            tempPath = Instantiate(drawnPathPrefab, gameObject.transform).GetComponent<DrawnPath>();
            tempPath.Initialize(Camera.main.ScreenToWorldPoint(points[0]), Camera.main.ScreenToWorldPoint(points[1]));

            for (int i = 2; i < points.Count; i++)
            {
                tempPath.AddPoint(Camera.main.ScreenToWorldPoint(points[i]));
            }


            string recognised = gesture.Recognise(path);

            // RESULT OF GESTURE RECOGNITION HERE
            // EG create circle/line etc

            drawing = false;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(previousMousePos, currentMousePos) > mouseMoveThreshold)
            {
                if (!drawing)
                {
                    currentPath = Instantiate(drawnPathPrefab, gameObject.transform).GetComponent<DrawnPath>();
                    currentPath.Initialize(previousMousePos, currentMousePos);
                    drawing = true;
                    previousMousePos = currentMousePos;
                } else
                {
                    currentPath.AddPoint(currentMousePos);
                    previousMousePos = currentMousePos;
                }
            }
        } else
        {
            drawing = false;
        }
	}
}
