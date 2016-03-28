using UnityEngine;
using System.Collections;
using System;

namespace Song
{
    public class RegularNote : MonoBehaviour
    {

        Note note;
 
        // Use this for initialization
        void Start()
        {
            note = transform.GetComponent<Note>();
        }


        void Update()
        {
            note.UpdateNote();
            if (note.OutOfSongBounds())
            {
                note.DestroyNote(false);
            }
            if (Input.GetMouseButtonDown(0) && note.InTrailClickBounds() && note.InTouchInputBounds())
            {
                note.DestroyNote(true);
            }
        }

    }
}