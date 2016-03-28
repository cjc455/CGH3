using UnityEngine;
using System.Collections;

namespace Song
{
    public class Note : MonoBehaviour
    {
        public NoteEntry noteEntry;
        Trail trail;
        static SongController songController;
        static GameVariables gameVars;
        static Song song;
        void Start()
        {
            if(songController == null)
            {
                songController = MSingleton.GetSingleton<SongController>();//MonoSingleton.GetSingleton("Song").GetComponent<SongController>();
            }
            if (gameVars == null)
            {
                gameVars = MSingleton.GetSingleton<GameVariables>();//MonoSingleton.GetSingleton("GameVariables").GetComponent<GameVariables>();
            }
            if(song == null)
            {
                song = MSingleton.GetSingleton<Song>(); //MonoSingleton.GetSingleton("Playing Song").GetComponent<Song>();
            }
            trail = songController.noteTrails[noteEntry.trailIndex];
            UpdateNote(0);
            OnStartNote();
        }

        public Trail GetTrail()
        {
            return trail;
        }
        public void SetNoteEntry(NoteEntry noteEntry)
        {
            Debug.Log("Set NOte Entry");
            this.noteEntry = noteEntry;
        }
        public NoteEntry GetNoteEntry()
        {
            return noteEntry;
        }
        void Update()
        {

        }
        protected virtual void OnUpdateNote() { }
        protected virtual void OnStartNote() { }

        public Vector3 GetSlideNotePosition()
        {
            Vector3 direction = Vector3.zero;
            if(noteEntry.slidePosition > 0 && noteEntry.slidePosition <= 1)
            {
                direction =  songController.noteTrails[1].end.position - songController.noteTrails[0].end.position;
            }
            else if (noteEntry.slidePosition > 1 && noteEntry.slidePosition <= 2)
            {
                direction = songController.noteTrails[2].end.position - songController.noteTrails[1].end.position ;
            }
           // Debug.Log(direction);
            return direction * noteEntry.slidePosition;
            

        }
        private void SetRotation()
        {
            if ((noteEntry.parentNote != null && noteEntry.parentNote.GetNoteType() == NoteEntry.NoteType.Slide) || noteEntry.GetNoteType() == NoteEntry.NoteType.Slide)
            {
                

            }
        }
        public void UpdateNote()
        {
            UpdateNote(song.GetTimeInSong());
        }
        public void UpdateNote(float timeInSong)
        {
            
            if(trail == null) { return;  }
            //set position
            Vector3 newPos =
                   trail.start.position
                   + Vector3.Normalize(trail.end.position - trail.start.position)
                   * (timeInSong - noteEntry.startTime + songController.GetNoteTime())
                   * (Vector3.Distance(trail.start.position, trail.end.position))
                   / (songController.GetNoteTime());

            if((noteEntry.parentNote != null && noteEntry.parentNote.GetNoteType() == NoteEntry.NoteType.Slide) || noteEntry.GetNoteType() == NoteEntry.NoteType.Slide)
            {
                newPos += GetSlideNotePosition();

            }
            newPos.z -= .5f;
            transform.position = newPos;
            //set transparency
            float transparency = Vector3.Distance(newPos, trail.start.position) / songController.GetNoteTransparenctFadeTime();
            transparency = Mathf.Clamp(transparency, 0f, 1f);
            Color color = GetComponent<Renderer>().material.color;
            color.a = transparency;
            GetComponent<Renderer>().material.color = color;
            //Debug.Log("trans " + transparency);

            //if children notes > 0, then

            
        }
        public void DestroyNote(bool clickedSuccess)
        {
            if(clickedSuccess)
            {
               // gameVars.UpdateScore(songController.GetNoteClickScoreChange());
                Instantiate(trail.noteClickSFX, transform.position, Quaternion.identity);
                //  gameVars.UpdateScoreNoteClick(noteEntry);
                MSingleton.GetSingleton<GameScore>().NoteClick();
                //Debug.Log("Note clicked at " + song.GetTimeInSong() + ", but was supposed to be at " + noteEntry.startTime);

                noteEntry.ApplyDecoration();
            }
            else
            {
                MSingleton.GetSingleton<GameScore>().NoteFail();
                // gameVars.UpdateScore(songController.GetNoteFailScoreChange());
                //gameVars.UpdateScoreNoteFail(noteEntry);
            }
            Destroy(gameObject);
        }
        public bool OutOfSongBounds()
        {
            if(songController.noteTrails == null)
            {
                Debug.Log("Trail Null");
            }
            if(transform == null)
            {
                Debug.Log("Song controller is null");
            }
            if(this.gameObject == null) { Debug.Log("Gameobject null."); }


            if (transform.position.y  <= songController.noteTrails[noteEntry.trailIndex].end.position.y - songController.GetNoteFailRange())
            {
                return true;
            }
            return false;
        }
        public bool AfterClickPoint()
        {
            if (transform.position.y <= trail.end.position.y)
            {
                return true;
            }
            return false;
        }
        public bool InTrailClickBounds()
        {
            if (transform.position.y >= trail.end.position.y - songController.GetNoteClickRange() &&
                transform.position.y <= trail.end.position.y + songController.GetNoteClickRange())
            {
                return true;
            }
            return false;
        }
        public bool InTouchInputBounds()
        {
            Vector2 input = Input.mousePosition;
            Vector2 noteInScreen = Camera.main.WorldToScreenPoint(transform.position);

               // Debug.Log("Note " + noteInScreen.ToString());
            if (Vector2.Distance(noteInScreen, input) < 100)
            {
              //  Debug.Log("Mouse " + input.ToString() + "  Note " + noteInScreen.ToString() + "  Dist " + Vector2.Distance(noteInScreen, input));
                return true;
               

            }

            return false;
        }

        public bool InTrailYTransitionEndBounds()
        {
            if(transform.position.y <= trail.end.position.y - songController.GetNoteYTransitionEnd())
            {
                return true;
            }
            return false;
        }
        
    }
}