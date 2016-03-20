using UnityEngine;
using System.Collections;

namespace Song
{
    public class Note : MonoBehaviour
    {
        NoteEntry noteEntry;
        Trail trail;
        static SongController songController;
        static GameVariables gameVars;
        static Song song;
        void Start()
        {
            if(songController == null)
            {
                songController = MonoSingleton.GetSingleton("Song").GetComponent<SongController>();
            }
            if (gameVars == null)
            {
                gameVars = MonoSingleton.GetSingleton("GameVariables").GetComponent<GameVariables>();
            }
            if(song == null)
            {
                song = MonoSingleton.GetSingleton("Playing Song").GetComponent<Song>();
            }
            trail = songController.noteTrails[noteEntry.trailIndex];
            UpdateNote(0);
        }
        public Trail GetTrail()
        {
            return trail;
        }
        public void SetNoteEntry(NoteEntry noteEntry)
        {
            this.noteEntry = noteEntry;
        }
        public NoteEntry GetNoteEntry()
        {
            return noteEntry;
        }
        void Update()
        {
           // float timeInSong = song.GetTimeInSong();
           // Debug.Log(timeInSong);
           // UpdateNote(timeInSong);

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
                   * (timeInSong - noteEntry.startTime)
                   * (Vector3.Distance(trail.start.position, trail.end.position))
                   / (songController.GetNoteTime());

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
                Instantiate(trail.noteClickSFX, trail.end.position, Quaternion.identity);
            }
            else
            {
               // gameVars.UpdateScore(songController.GetNoteFailScoreChange());
            }
            Destroy(gameObject);
        }
        public bool OutOfSongBounds()
        {
            if (transform.position.z + songController.GetNoteClickRange() <= trail.end.position.z)
            {
                return true;
            }
            return false;
        }
        public bool InTrailClickBounds()
        {
            if (transform.position.z + songController.GetNoteClickRange() >= trail.end.position.z &&
                transform.position.z - songController.GetNoteClickRange() <= trail.end.position.z)
            {
                return true;
            }
            return false;
        }
        public bool InSlideInputBoundsXY()
        {
            return true;
        }
        public bool InSlideInputBoundsZ()
        {
            
            if(transform.position.z <= trail.end.position.z)
            {
                return true;
            }
            return false;
        }
        
    }
}