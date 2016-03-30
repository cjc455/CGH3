using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Song
{
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
        public enum NoteType { Regular, Transition, Long, Slide, Shake, Empty }
        public float startTime;
        //public float duration;
        public int trailIndex;
        public float slidePosition;
        public GameObject gameObject;
        public NoteEntry parentNote;
        public float length = 0;
        private NoteType noteType;
        static SongController songController;

        //TODO: create class NoteDecoration
        //public NoteDecoration noteDecoration;

        //NoteEntry ne = new SlideNoteEntry(time, points, noteDecoration);
        //Won't need getters for these, the gameObjects needed by them will be accessed inside the note class


        public Color localColor;
        public Color globalColor = Color.cyan;
        public float audioLength = .2f;
       // public float audioSpeed = 1;
        public float cameraShake;
        public float localTrailScale;
        public float globalTrailScale = .05f;
        public float audioMagnitude = 1;
        public float blurAmmount;
        const bool playOnClickedNotesOnly = true;
        public float bloomAmmount = .25f;
        float twirlAmmount = 5;
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
            //    songController = MonoSingleton.GetSingleton("Song").GetComponent<SongController>();
                songController = MSingleton.GetSingleton<SongController>();
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
        public static NoteEntry Empty(float startTime)
        {
            NoteEntry newNoteEntry = new NoteEntry(startTime, NoteType.Empty);
            return newNoteEntry;
        }
        /*
        TODO: only make the notes based on the pitch in the song
        public static NoteEntry[] Slide(float startTime, float length, float minTrailPos, float maxTrailPos)
        {

        }
        */
        public bool InSong()
        {
            float offset = 0;
            if(noteType != NoteType.Empty)
            {
                offset = MSingleton.GetSingleton<SongController>().GetNoteTime();
            }
            bool result = (startTime - offset <= MSingleton.GetSingleton<Song>().GetTimeInSong());

            if(result && noteType == NoteType.Empty)
            {
                ApplyDecoration();
            }
            return result;
        }
        List<SongSFX> sfx = new List<SongSFX>();
        public void ApplyDecoration()
        {
            Debug.Log("Apply decoration for " + noteType.ToString());
            /*
            if (globalColor != null)
            {
                MSingleton.GetSingleton<SongSFX>().GlobalLight(this);
            }
            if (globalTrailScale != 0)
            {
                MSingleton.GetSingleton<SongSFX>().GlobalScale(this);
            }
            MSingleton.GetSingleton<SongSFX>().Bloom(this);
            */
           

            if (sfxValues != null)
            {
                foreach (SongSFXValue v in sfxValues)
                {
                    MSingleton.GetSingleton<SongSFXController>().AddSFX(v);
                }
            }
        }
        List<SongSFXValue> sfxValues;
        public void AddSFX( SongSFXValue value)
        {
            if(sfxValues == null)
            {
                sfxValues = new List<SongSFXValue>();
            }
            sfxValues.Add(value);
        }
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
          
           

            //First create long note
            NoteEntry newNoteEntry = new NoteEntry(startTime, NoteType.Long);
            newNoteEntry.length = length;
            
            newNoteEntry.slidePosition = (points[0].GetPosition());
            newNoteEntry.noteType = NoteType.Slide;


            Vector3[] positions = new Vector3[points.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = new Vector3(points[i].GetPosition(), points[i].GetTime(), 0);
            }
            positions = Curver.MakeSmoothCurve(positions, 3.0f);
            int numberOfEntries = positions.Length + 1;
            NoteEntry[] newNoteEntries = new NoteEntry[numberOfEntries];
            newNoteEntries[0] = newNoteEntry;
            float entriesPerDistance = 15;
            for (int i = 1; i < newNoteEntries.Length - 1; i++)
            {
                // float transitionNoteStartTime = startTime + positions[i - 1].y;
                
                 float transitionNoteStartTime = startTime + i * (length) / entriesPerDistance;
                 newNoteEntries[i] = Transition(transitionNoteStartTime, 0, newNoteEntries[0]);
                 newNoteEntries[i].slidePosition = positions[i - 1].x;
                 

                /*

                d = sqrt ( x2 + y2 )
                we know y, find x

                x = sqrt( d2 - y2 )

                */

                /*
                float transitionNoteStartTime = startTime + i * (length) / entriesPerDistance;
                float d = Vector3.Distance(positions[i - 1], positions[i]);
                float y = transitionNoteStartTime;
               

                float transitionNoteStartX = Mathf.Sqrt(Mathf.Pow(d, 2) - Mathf.Pow(y, 2));
                newNoteEntries[i] = Transition(transitionNoteStartTime, 0, newNoteEntries[0]);
                newNoteEntries[i].slidePosition = transitionNoteStartX;
                */

            }

            return newNoteEntries;
        }

        /*
        All data for the note is created in one of the above methods
        This method just instansiates it. Not much need for data setting/manipulation
        */
        GameState playingGameState;
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
                    gameObject = Object.Instantiate(GetSongController().noteTrails[trailIndex].longNoteEndObject);
                    gameObject.AddComponent<LongNote>();
                    gameObject.transform.parent = GetSongController().transform;
                    break;
            }

            if (noteType != NoteType.Empty)
            {
                gameObject.name = noteType.ToString();
                gameObject.transform.position = GetSongController().noteTrails[trailIndex].start.position;

                Note newNote = gameObject.AddComponent<Note>();
                newNote.SetNoteEntry(this);
            }
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

        [SerializeField]
        float previewStartTime;

        float desiredNoteTime;
        float currentNoteTime;

        public AudioClip GetSongClip()
        {
            return songClip;
        }
        public float GetPreviewStartTime()
        {
            return previewStartTime;
        }
        public void SetDesiredNoteTime(float desiredNoteTime)
        {
            this.desiredNoteTime = desiredNoteTime;
        }
        public void SetScoreNoteTimeAdd(float value)
        {

        }
        const float noteTransparencyFadeTime = 3f;
        public string GetName() { return songName; }
        public int GetUIOrder() { return uiOrder;  }
        public SongDifficulty GetDifficulty() { return songDifficulty;  }
        public string GetArtist() { return songArtist;  }
        private float timeInSong = 0;
        public float GetTimeInSong() { return timeInSong; }
        List<NoteEntry> noteEntries;
        List<NoteEntry> songDecorations;
        //Next note that will be instansiated
        NoteEntry nextNoteEntry;
        //Only keep track of instansiated notes . . .
        //less complex than storing all the notes
        //No "activate/deactivate", keeping track of what is instansiated and whats not, etc.
        

        Trail[] trails;
        float songStartTime;
        GameVariables gameVars;
        SongController songController;

        void AddSongNoteEntry()
        {

        }
        private List<NoteEntry> GetSong2NoteEntries()
        {
            
            List<NoteEntry> n = new List<NoteEntry>();
            n.Add(NoteEntry.Regular(0, 2));
            n.Add(NoteEntry.Regular(1, 1));
            n.Add(NoteEntry.Regular(1.5f, 1));
            n.Add(NoteEntry.Regular(3, 0));
            n.AddRange(NoteEntry.Long(3.5f, 1, 1));
            return n;
        }
        private List<NoteEntry> GetSong3NoteEntries()
        {
            List<NoteEntry> n = new List<NoteEntry>();
            n.Add(NoteEntry.Regular(0, 2));
            n.Add(NoteEntry.Regular(1, 1));
            n.Add(NoteEntry.Regular(1.5f, 1));
            n.Add(NoteEntry.Regular(3, 0));
            n.AddRange(NoteEntry.Long(3.5f, 1, 1));
            return n;
        }
        private List<NoteEntry> GetSong1NoteEntries()
        {
           
            List<NoteEntry> n = new List<NoteEntry>();
            
            for(int i = 0; i < 400; i++)
            {
                //     Debug.Log("1");
                // n.Add(NoteEntry.Regular(i * .1f + 2f, i % 3));
                //  NoteEntry newNoteEntry = NoteEntry.Regular(i * 2f, i % 3);
                //newNoteEntry.AddSFX(SongSFXValue.FactoryInstant(SongSFXType.Bloom, SongSFXValue.MathFunction.Sine, i * 2f, -.25f, 5f));
                //  n.Add(newNoteEntry);
                NoteEntry newNoteEntry2 = NoteEntry.Regular(1 * 2f, i % 3);
                newNoteEntry2.AddSFX(SongSFXValue.FactoryInstant(SongSFXType.Bloom, SongSFXValue.MathFunction.Constant, i * 2f, -.55f, 2f));
                n.Add(newNoteEntry2);
                if (i % 4 == 0)
                {
                    NoteEntry newNoteEntry = NoteEntry.Regular(1 * 2f, i % 3);
                    newNoteEntry.AddSFX(SongSFXValue.FactoryInstant(SongSFXType.Bloom, SongSFXValue.MathFunction.Constant, i * 2f, -.25f, 2f));
                    n.Add(newNoteEntry);

                }
               else if(i % 4 == 1)
                {
                    n.AddRange(NoteEntry.Long(i * 2f, i % 3, 1f));
                }
               else if (i % 4 == 2)
                {
                    
                    NoteEntry impactEffect = NoteEntry.Empty(i * 2f);
                    impactEffect.AddSFX(SongSFXValue.FactoryInstant(SongSFXType.GlobalTrailScale, SongSFXValue.MathFunction.Sine, i * 2f, .2f, .25f));
                    impactEffect.AddSFX(SongSFXValue.FactoryInstant(SongSFXType.Bloom, SongSFXValue.MathFunction.Sine, i * 2f, -.2f, .25f));
                    n.Add(impactEffect);


                }
               else
                {
                    SlideNotePoint[] points = new SlideNotePoint[7];
                    for( float j = 0; j < points.Length; j++) {
                        points[(int)j] = new SlideNotePoint(j * 2 / points.Length, Random.Range(0f, 1f) * 2f);
                    }
                    n.AddRange(NoteEntry.Slide(i * 2f, points));
                }
                
                
              //  n.Add(new NoteEntry(i * .5f + 2f, i % 3, ((i % 5) * .75f)));
            }
            
            return n;
        }
        private int GetNextNoteEntryIndex()
        {
            //Problem: when nextNoteEntry is null, returns 0
            return noteEntries.FindIndex(n => n == nextNoteEntry);
        }
        /*
        how to offset note positions to instansiate them before the song:
        1. 
        in class Song
        while(nextNoteEntry != null && nextNoteEntry.startTime - songController.GetNoteTime() <= GetTimeInSong()) 
            {

        2.
        In class Note
        Vector3 newPos =
                   trail.start.position
                   + Vector3.Normalize(trail.end.position - trail.start.position)
                   * (timeInSong - noteEntry.startTime + songController.GetNoteTime())
                   * (Vector3.Distance(trail.start.position, trail.end.position))
                   / (songController.GetNoteTime());

        */
        const float endTimeLength = 3;
        float endTime = 0;
        bool finished = false;
        public void UpdateSong()
        {
            timeInSong += Time.deltaTime;
           // Debug.Log("Update Song");
            if (nextNoteEntry == null) { Debug.Log("Null"); }
            // else { Debug.Log(nextNoteEntry.startTime.ToString()); }
            //nextNoteEntry.InSong() { nextNoteEntry.startTime - songController.GetNoteTime() <= GetTimeInSong() }
            
            while (nextNoteEntry != null && nextNoteEntry.InSong()) 
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
                    finished = true;
                    
                }

            }
            if(endTime == 0)
            {
                if(songController.GetComponentsInChildren<Note>().Length == 0 && finished)
                {
                    endTime = GetTimeInSong();
                }
            }
            else if(GetTimeInSong() >= endTime + endTimeLength)
            {
                Debug.Log("Hi");
                if (finished)
                {
                    Debug.Log("yo");
                    MSingleton.GetSingleton<GameStateController>().SetGameState("Song Finish");
                }
            }
            //Any notes failed?
        }
        public void StartSong()
        {
            gameVars = MSingleton.GetSingleton<GameVariables>(); //MonoSingleton.GetSingleton("GameVariables").GetComponent<GameVariables>();
            songController = MSingleton.GetSingleton<SongController>();// MonoSingleton.GetSingleton("Song").GetComponent<SongController>();
            trails = songController.noteTrails;
            songStartTime = Time.time;
            timeInSong = 0;
            //TODO: throw error if notes are not ordered by accending startTime
            noteEntries = GetSong1NoteEntries();

            Debug.Log("Start song");
            nextNoteEntry = noteEntries[0];
            

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