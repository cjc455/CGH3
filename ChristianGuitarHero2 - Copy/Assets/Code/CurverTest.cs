using UnityEngine;
using System.Collections;

public class CurverTest : MonoBehaviour {

    public Vector3[] points;

    public LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2  = Color.red;
 
 void Start()
    {
        points = Curver.MakeSmoothCurve(points, 3.0f);

        lineRenderer.SetColors(c1, c2);
        lineRenderer.SetWidth(0.5f, 0.5f);
        lineRenderer.SetVertexCount(points.Length);
        int counter = 0;
        foreach (Vector3 p in points)
        {
            lineRenderer.SetPosition(counter, p);
            ++counter;
        }
    }
}
