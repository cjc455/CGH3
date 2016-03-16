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

        const float noteTime = .5f;
        const float noteClickRange = .5f;
        const float noteFailZRange = 2f;
        const float noteFailScoreChange = -1;
        const float noteClickScoreChange = 1;
        // Use this for initialization
        void Start() {
            
            

            

        }
        private GameObject InstansiateNote(NoteEntry noteEntry)
        {
            GameObject newNoteGameObject = Instantiate(trails[noteEntry.trailIndex].noteObject);

            newNoteGameObject.transform.position = trails[noteEntry.trailIndex].start.position;

            noteEntry.gameObject = newNoteGameObject;
            noteEntry.gameObject.transform.parent = transform;
            Debug.Log("Note Created");
            return newNoteGameObject;
        }
        private void DestroyNote(NoteEntry noteEntry, float scoreChange)
        {
            Destroy(noteEntries[0].gameObject);
            gameVars.UpdateScore(scoreChange);
            noteEntries.Remove(noteEntries[0]);
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
            //Is it time to add a new note?
            float timeInSong = Time.time - songStartTime;

           // Debug.Log("Update Song");
            if (nextNoteEntry == null) { Debug.Log("Null"); }
           // else { Debug.Log(nextNoteEntry.startTime.ToString()); }
            
            
            while(nextNoteEntry != null && nextNoteEntry.startTime <= timeInSong) 
            {
                
                GameObject newNoteObject = InstansiateNote(nextNoteEntry);
               
                if(!newNoteObject.GetComponent<Note>())
                {
                    newNoteObject.AddComponent<Note>();
                }
                newNoteObject.GetComponent<Note>().InitializeNote(nextNoteEntry, trails[nextNoteEntry.trailIndex]);

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
            
            

            //Update note positions

            //Sloppy way of doing it . . . but too much effort required to fix
           
            for(int i = 0; i < GetNumberOfInstansiatedNotes(); i++)
            {
                //set position
                Vector3 newPos = 
                    trails[noteEntries[i].trailIndex].start.position 
                    + Vector3.Normalize(trails[noteEntries[i].trailIndex].end.position - trails[noteEntries[i].trailIndex].start.position)
                    * (timeInSong - noteEntries[i].startTime) 
                    * (Vector3.Distance(trails[noteEntries[i].trailIndex].start.position, trails[noteEntries[i].trailIndex].end.position)) 
                    / (noteTime);

                noteEntries[i].gameObject.transform.position = newPos;

                

                //set transparency
                float transparency = Vector3.Distance(newPos, trails[noteEntries[i].trailIndex].start.position) / noteTransparencyFadeTime;
                transparency = Mathf.Clamp(transparency, 0f, 1f);
                Color color = noteEntries[i].gameObject.GetComponent<Renderer>().material.color;
                color.a = transparency;
                noteEntries[i].gameObject.GetComponent<Renderer>().material.color = color;
                Debug.Log("trans " + transparency);

            }
            
            //Any notes clicked?
            //slight inefficiency, but really insignificant performance change
            for(int i = 0; i < trails.Length; i++)
            {
                
                if (Input.GetKeyDown(trails[i].keyCode))
                {
                    //this works
                    // Debug.Log(trails[i].keyCode.ToString());
                    int len = GetNumberOfInstansiatedNotes();
                    for (int j = 0; j < len; j++)
                    {
                        if(noteEntries[j].trailIndex == i && InClickBounds(noteEntries[j])) {
                            DestroyNote(noteEntries[j], noteClickScoreChange);
                            len -= 1;
                            Debug.Log("NOte Clicked");
                        }
                    }
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
        private int GetNumberOfInstansiatedNotes()
        {
            int len = 0;
            if (nextNoteEntry == null) { len = noteEntries.Count; }
            else { len = GetNextNoteEntryIndex(); }

            return len;
        }
        
    }
    
}