using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Song
{
    public enum SongDifficulty
    {
        Easy,
        Medium,
        Hard
    }
    public class NoteEntry
    {
        public float startTime;
        //public float duration;
        public int trailIndex;
        public GameObject gameObject;

        public NoteEntry(float startTime, int trailIndex)
        {
            this.startTime = startTime;
            this.trailIndex = trailIndex;
        }
    }
    public class Song : MonoBehaviour {

        [SerializeField]
        string songName;

        [SerializeField]
        string songArtist;

        [SerializeField]
        SongDifficulty songDifficulty;

        [SerializeField]
        int uiOrder;

        const float noteTransparencyFadeTime = 3f;
        public string GetName() { return songName; }
        public int GetUIOrder() { return uiOrder;  }
        public SongDifficulty GetDifficulty() { return songDifficulty;  }
        public string GetArtist() { return songArtist;  }
        public float GetTimeInSong() { return Time.time - songStartTime; }
        List<NoteEntry> noteEntries;
        //Next note that will be instansiated
        NoteEntry nextNoteEntry;
        //Only keep track of instansiated notes . . .
        //less complex than storing all the notes
        //No "activate/deactivate", keeping track of what is instansiated and whats not, etc.
        

        Trail[] trails;
        float songStartTime;
        GameVariables gameVars;
        SongController songController;

       
        private GameObject InstansiateNote(NoteEntry noteEntry)
        {
            GameObject newNoteGameObject = Instantiate(trails[noteEntry.trailIndex].noteObject);

            newNoteGameObject.transform.position = trails[noteEntry.trailIndex].start.position;

            noteEntry.gameObject = newNoteGameObject;
            noteEntry.gameObject.transform.parent = transform;
            Note newNote = noteEntry.gameObject.AddComponent<Note>();
            newNote.InitializeNote(noteEntry, trails[noteEntry.trailIndex], this);

            if(noteEntry == null)
            {
                Debug.LogError("Note Entry is null.");
            }
          //  Debug.Log("Note Created");
            return newNoteGameObject;
        }

        private List<NoteEntry> GetSong1NoteEntries()
        {
            List<NoteEntry> n = new List<NoteEntry>();
            for(int i = 0; i < 400; i++)
            {
                n.Add(new NoteEntry(i * .5f + 2f, i % 3));
            }
            
            return n;
        }
        private int GetNextNoteEntryIndex()
        {
            //Problem: when nextNoteEntry is null, returns 0
            return noteEntries.FindIndex(n => n == nextNoteEntry);
        }
        public void UpdateSong()
        {

           // Debug.Log("Update Song");
            if (nextNoteEntry == null) { Debug.Log("Null"); }
           // else { Debug.Log(nextNoteEntry.startTime.ToString()); }
            
            
            while(nextNoteEntry != null && nextNoteEntry.startTime <= GetTimeInSong()) 
            {
                
                GameObject newNoteObject = InstansiateNote(nextNoteEntry);
               
                if(noteEntries.Count > 0 && GetNextNoteEntryIndex() + 1 < noteEntries.Count)
                {
                    nextNoteEntry = noteEntries[GetNextNoteEntryIndex() + 1];
                    // nextNoteEntry = null;
                }
                else
                {
                    nextNoteEntry = null;
                }

            }
 

            //Any notes failed?
        }
        public void StartSong()
        {
            gameVars = MonoSingleton.GetSingleton("GameVariables").GetComponent<GameVariables>();
            songController = MonoSingleton.GetSingleton("Song").GetComponent<SongController>();
            trails = songController.noteTrails;

            //TODO: throw error if notes are not ordered by accending startTime
            noteEntries = GetSong1NoteEntries();

            Debug.Log("Start song");
            nextNoteEntry = noteEntries[0];
            songStartTime = Time.time;
        }
        public void StopSong()
        {

        }
        public void PauseSong()
        {

        }
        /*
        private int GetNumberOfInstansiatedNotes()
        {
            int len = 0;
            if (nextNoteEntry == null) { len = noteEntries.Count; }
            else { len = GetNextNoteEntryIndex(); }

            return len;
        }
        */
    }
    
}