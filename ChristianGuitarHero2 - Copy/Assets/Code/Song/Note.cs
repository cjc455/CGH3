using UnityEngine;
using System.Collections;

namespace Song
{
    public class Note : MonoBehaviour
    {
        NoteEntry noteEntry;
        Trail trail;
        public void InitializeNote(NoteEntry noteEntry, Trail trail)
        {
            this.noteEntry = noteEntry;
            this.trail = trail;
        }
        void Update()
        {

        }
        private bool OutOfSongBounds(NoteEntry note)
        {
            if (note == null)
            {
                Debug.LogError("NoteEntry null");
                return false;
            }
            if (note.gameObject == null)
            {
                Debug.LogError("note.gameObject is null");
                return false;
            }
            if (
                note.gameObject.transform.position.z <=
                trail.end.position.z - noteFailZRange)
            {
                return true;
            }
            return false;
        }
        private bool InClickBounds(NoteEntry note)
        {
            if (note == null)
            {
                Debug.LogError("NoteEntry null");
                return false;
            }
            if (note.gameObject == null)
            {
                Debug.LogError("note.gameObject is null");
                return false;
            }
            if (note.gameObject.transform.position.z + noteClickRange >= trails[note.trailIndex].end.position.z &&
                note.gameObject.transform.position.z - noteClickRange <= trails[note.trailIndex].end.position.z)
            {
                return true;
            }
            return false;
        }
    }
}