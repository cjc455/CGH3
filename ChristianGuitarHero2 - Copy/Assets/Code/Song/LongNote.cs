using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Song
{

    public class LongNote : MonoBehaviour
    {

        NoteEntry noteEntry;
        

        public LineRenderer lineRenderer;
        public Color c1 = Color.yellow;
        public Color c2 = Color.red;
        public List<Transform> childsOfGameobject = new List<Transform>();

        static bool useLineRenderer = false;
        void Start()
        {
            if(useLineRenderer)
                lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            childsOfGameobject = new List<Transform>();
            Transform[] transforms = GetComponentsInChildren<Transform>();
            Debug.Log("points list" + transforms.Length);
            Vector3[] points = new Vector3[transforms.Length - 1];
            Debug.Log("pointsarray " + points.Length);
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = transforms[i].position;
            }
            points = Curver.MakeSmoothCurve(points, 3.0f);

            if (useLineRenderer)
            {

                lineRenderer.SetColors(c1, c2);
                lineRenderer.SetWidth(0.5f, 0.5f);
                lineRenderer.SetVertexCount(points.Length);
                int counter = 0;

                foreach (Vector3 p in points)
                {
                    Debug.Log("Set Pos");
                    lineRenderer.SetPosition(counter, p);
                    ++counter;
                }
            }
        }
        /*
        private List<Transform> GetAllChilds(Transform transformForSearch)
        {
            foreach (Transform trans in transformForSearch.transform)
            {
                //Debug.Log (trans.name);
                GetAllChilds(trans);
                childsOfGameobject.Add(trans);
            }
            return getedChilds;
        }
        */
    }
}