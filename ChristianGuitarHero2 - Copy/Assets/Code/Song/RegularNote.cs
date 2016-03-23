using UnityEngine;
using System.Collections;


namespace Song
{
    public class RegularNote : MonoBehaviour
    {
        Note note;
        // Use this for initialization
        void Start()
        {
            note = GetComponent<Note>();
        }

        // Update is called once per frame
        void Update()
        {
            note.UpdateNote();
            if(note.OutOfSongBounds())
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