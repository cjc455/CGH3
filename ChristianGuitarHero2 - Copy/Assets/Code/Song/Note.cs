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
        Song song;
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
        }
        public void InitializeNote(NoteEntry noteEntry, Trail trail, Song song)
        {
           
            this.noteEntry = noteEntry;
            this.trail = trail;
            this.song = song;
            UpdateNote(0);
        }
        void Update()
        {
            float timeInSong = song.GetTimeInSong();
            UpdateNote(timeInSong);

        }
        private void UpdateNote(float timeInSong)
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

            if (Input.GetKeyDown(trail.keyCode) && InClickBounds())
            {
                DestroyNote(true);
            }
            else if (OutOfSongBounds())
            {
                DestroyNote(false);
            }
        }
        private void DestroyNote(bool clickedSuccess)
        {
            if(clickedSuccess)
            {
                gameVars.UpdateScore(songController.GetNoteClickScoreChange());
            }
            else
            {
                gameVars.UpdateScore(songController.GetNoteFailScoreChange());
            }
            Destroy(gameObject);
        }
        private bool OutOfSongBounds()
        {
            if (transform.position.z <= trail.end.position.z - songController.GetNoteFailZRange())
            {
                return true;
            }
            return false;
        }
        private bool InClickBounds()
        {
            if (transform.position.z + songController.GetNoteClickRange() >= trail.end.position.z &&
                transform.position.z - songController.GetNoteClickRange() <= trail.end.position.z)
            {
                return true;
            }
            return false;
        }
    }
}