﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Song {
    public class LongNoteController : MonoBehaviour {

        Note longNote;
        bool isClicking = false;

        // Use this for initialization
        void Start() {
            name = "LongNote Controller";
        }

        // Update is called once per 

        void SetSlideRotation(Transform target, int i)
        {
            GetTransitionNotes()[i].transform.LookAt(target);
            Vector3 angles = GetTransitionNotes()[i].transform.eulerAngles;
            angles.x -= 90;
            GetTransitionNotes()[i].transform.eulerAngles = angles;
        }
        void Update() {
            if(longNote == null)
            {
                
            }
            foreach (TransitionNote tn in GetTransitionNotes())
            {
                tn.GetComponent<Note>().UpdateNote();
                
                
            }
            for(int i = 1; i < GetTransitionNotes().Length; i++)
            {
               // Vector3 scale = GetTransitionNotes()[i].transform.localScale;
              //  scale.z = Vector3.Distance(GetTransitionNotes()[i].transform.position, GetTransitionNotes()[i - 1].transform.position);
               // GetTransitionNotes()[i].transform.localScale = scale;
            }
            if (longNote.GetNoteEntry().GetNoteType() == NoteEntry.NoteType.Slide)
            {
                if (GetTransitionNotes().Length > 0)
                {
                    SetSlideRotation(longNote.transform, 0);
                    for (int i = 1; i < GetTransitionNotes().Length; i++)
                    {
                        SetSlideRotation(GetTransitionNotes()[i - 1].transform, i);
                    }
                }
            }
            if (!isClicking)
            {
                longNote.UpdateNote();
                
                if (longNote.OutOfSongBounds())
                {
                    DestroyAll();

                }
                if(Input.GetMouseButtonDown(0) && longNote.InTouchInputBounds() && longNote.InTrailClickBounds())
                {
                    // longNote.DestroyNote(true);
                   
                    longNote.GetComponent<Renderer>().enabled = false;
                    isClicking = true;
                }
            }
            else
            {

                if (GetTransitionNotes().Length <= 0)
                {
                    DestroyAll();
                    
                    return;
                }
                
                if (!Input.GetMouseButton(0))
                {
                    DestroyAll();
                    
                    return;
                }
                if(GetTransitionNotes()[0].GetComponent<Note>().AfterClickPoint())
                {
                    if(GetTransitionNotes()[0].GetComponent<Note>().InTouchInputBounds())
                    {
                        
                        GetTransitionNotes()[0].GetComponent<Note>().DestroyNote(true);
                        
                    }
                    else
                    {
                        DestroyAll();
                        
                        return;
                    }
                }
            }
        }
        private void DestroyAll()
        {
            Destroy(longNote.gameObject);
            Destroy(gameObject);
        }
        public void Initialize(LongNote longNote)
        {
            this.longNote = longNote.GetComponent<Note>();
            isClicking = false;
            // this.transitionNotes = longNote.GetComponent<Note>().GetNoteEntry().transitionNotes;
        }
        private TransitionNote[] GetTransitionNotes()
        {
            return longNote.GetComponentsInChildren<TransitionNote>();
        }
        /*


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