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
    public class SlideNotePoint
    {
        float time;
        float position;

        public float GetTime() { return time; }
        public float GetPosition() { return position; }
        public SlideNotePoint(float time, float position)
        {
            this.time = time;
            this.position = position;
        }
    }
    public class NoteEntry
    {
        public enum NoteType { Regular, Transition, Long, Slide, Shake }
        public float startTime;
        //public float duration;
        public int trailIndex;
        public float slidePosition;
        public GameObject gameObject;
        public NoteEntry parentNote;
        public float length = 0;
        private NoteType noteType;
        static SongController songController;
        public NoteType GetNoteType() { return noteType; }
        public SlideNotePoint[] GetSlideNotePoints() { return slideNotePoints; }
      //  public List<TransitionNote> transitionNotes;

        //Doesn't work, can only create new notes with the gameObject.add function
        //Component noteComponent

        SlideNotePoint[] slideNotePoints = null;

        private NoteEntry(float startTime, NoteType noteType)
        {
            this.startTime = startTime;
            this.noteType = noteType;
        }
        private static SongController GetSongController()
        {
            if(!songController)
            {
                songController = MonoSingleton.GetSingleton("Song").GetComponent<SongController>();
            }
            return songController;
        }
        public static NoteEntry Regular(float startTime, int trailIndex)
        {
            NoteEntry newNoteEntry = new NoteEntry(startTime, NoteType.Regular);
            newNoteEntry.trailIndex = trailIndex;
            newNoteEntry.noteType = NoteType.Regular;
            return newNoteEntry;
        }
        public static NoteEntry[] Long(float startTime, int trailIndex, float length)
        {
            float entriesPerDistance = 15;
            int numberOfEntries = (int)((length * entriesPerDistance) + 1);
            NoteEntry[] newNoteEntries = new NoteEntry[numberOfEntries];

            //First create long note
            NoteEntry newNoteEntry = new NoteEntry(startTime, NoteType.Long);
            newNoteEntry.length = length;
            newNoteEntry.trailIndex = trailIndex;
            newNoteEntry.noteType = NoteType.Long;
            newNoteEntries[0] = newNoteEntry;

          //  newNoteEntry.transitionNotes = new List<TransitionNote>();

            //now create transition notes
            for(int i = 1; i < newNoteEntries.Length; i++)
            {
                float transitionNoteStartTime = startTime + i * (length) / numberOfEntries;
                newNoteEntries[i] = Transition(transitionNoteStartTime, trailIndex, newNoteEntries[0]);
               // newNoteEntry.transitionNotes.Add(newNoteEntries[i]);
            }

            return newNoteEntries;
        }
        public static NoteEntry Transition(float startTime, int trailIndex, NoteEntry parentNote)
        {
            NoteEntry newNoteEntry = new NoteEntry(startTime, NoteType.Transition);
            newNoteEntry.trailIndex = trailIndex;
            newNoteEntry.noteType = NoteType.Transition;
            newNoteEntry.parentNote = parentNote;
            return newNoteEntry;

        }
        /*
        TODO: only make the notes based on the pitch in the song
        public static NoteEntry[] Slide(float startTime, float length, float minTrailPos, float maxTrailPos)
        {

        }
        */
        public static NoteEntry[] Slide(float startTime, SlideNotePoint[] points)
        {
            if(points == null)
            {
                Debug.LogError("points is null.");
                return null;
            }
            if(points.Length < 1)
            {
                Debug.LogError("points has length < 1. Please use a regularNote or longNote type");
                return null;
            }

            float length = points[points.Length - 1].GetTime() - points[0].GetTime();
            float entriesPerDistance = 15;
            int numberOfEntries = (int)((length * entriesPerDistance) + 1);
            NoteEntry[] newNoteEntries = new NoteEntry[numberOfEntries];

            //First create long note
            NoteEntry newNoteEntry = new NoteEntry(startTime, NoteType.Long);
            newNoteEntry.length = length;
            
            newNoteEntry.slidePosition = (points[0].GetPosition());
            newNoteEntry.noteType = NoteType.Long;
            newNoteEntries[0] = newNoteEntry;

            //  newNoteEntry.transitionNotes = new List<TransitionNote>();

            //now create transition notes
            for (int i = 1; i < newNoteEntries.Length; i++)
            {
                float transitionNoteStartTime = startTime + i * (length) / numberOfEntries;
                newNoteEntries[i] = Transition(transitionNoteStartTime, 0, newNoteEntries[0]);
                newNoteEntries[i].slidePosition = 0;
                // newNoteEntry.transitionNotes.Add(newNoteEntries[i]);
            }

            return newNoteEntries;
        }

        /*
        All data for the note is created in one of the above methods
        This method just instansiates it. Not much need for data setting/manipulation
        */
        public void InstansiateNote()
        {
            
            
            //newNote.InitializeNote(this, songController.noteTrails[trailIndex], this);

            switch (noteType)
            {
                case NoteType.Regular:
                    gameObject = Object.Instantiate(GetSongController().noteTrails[trailIndex].noteObject);
                    gameObject.AddComponent<RegularNote>();
                    gameObject.transform.parent = GetSongController().transform;
                    break;
                case NoteType.Long:
                    gameObject = Object.Instantiate(GetSongController().noteTrails[trailIndex].longNoteEndObject);
                    gameObject.AddComponent<LongNote>();
                    gameObject.transform.parent = GetSongController().transform;
                    break;
                case NoteType.Transition:
                    if(parentNote == null || parentNote.gameObject == null) { return; }
                    gameObject = Object.Instantiate(GetSongController().transitionNote);
                    gameObject.AddComponent<TransitionNote>();
                    gameObject.transform.parent = parentNote.gameObject.transform;
                   
                    break;
                case NoteType.Slide:
                    //  gameObject.AddComponent<SlideNote>();
                    break;
            }

            gameObject.name = noteType.ToString();
            gameObject.transform.position = GetSongController().noteTrails[trailIndex].start.position;
            
            Note newNote = gameObject.AddComponent<Note>();
            newNote.SetNoteEntry(this);
            //  Debug.Log("Note Created");


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

        [SerializeField]
        AudioClip songClip;

        

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

       
       

        private List<NoteEntry> GetSong1NoteEntries()
        {
            List<NoteEntry> n = new List<NoteEntry>();
            for(int i = 0; i < 400; i++)
            {
               // n.Add(NoteEntry.Regular(i * .1f + 2f, i % 3));
                n.AddRange(NoteEntry.Long(i * 2f,  i % 3, 1));
                
              //  n.Add(new NoteEntry(i * .5f + 2f, i % 3, ((i % 5) * .75f)));
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

                nextNoteEntry.InstansiateNote();
               
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

            AudioSource audioSource = songController.GetComponent<AudioSource>();
            audioSource.clip = songClip;
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