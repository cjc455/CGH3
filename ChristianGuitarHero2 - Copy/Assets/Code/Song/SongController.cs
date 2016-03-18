using UnityEngine;
using System;
using System.Collections;

namespace Song
{

    public class SongController : MonoBehaviour
    {
        public Trail[] noteTrails;
        public GameObject transitionNote;
        public Song[] songs;
        public GameObject clickSFX;
        [SerializeField]
        float noteTime = .5f;
        [SerializeField]
        float noteClickRange = .5f;
     //   [SerializeField]
     //   float noteFailZRange = 2f;
        [SerializeField]
        float noteFailScoreChange = -1;
        [SerializeField]
        float noteClickScoreChange = 1;
        [SerializeField]
        float noteTransparencyFadeTime = 2;

        AudioSource audioSource;

        private Song currentSong;
        Song songType;
        public float GetNoteTime(){ return noteTime; }
        public float GetNoteClickRange() { return noteClickRange;}

        public float GetNoteFailScoreChange() { return noteFailScoreChange; }
        public float GetNoteClickScoreChange() { return noteClickScoreChange; }
        public float GetNoteTransparenctFadeTime() { return noteTransparencyFadeTime; }
        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            GameStateController gameStateController = MonoSingleton.GetSingleton("GameState").GetComponent<GameStateController>();
            GameState playing = gameStateController.GetGameState("Playing");
            gameStateController.AddGameStateMessage(playing, GameStateEventMessage.Start, CreateSongObject);
            gameStateController.AddGameStateMessage(playing, GameStateEventMessage.Update, UpdateSong);
            GameState mainMenu = gameStateController.GetGameState("Main Menu");
            gameStateController.AddGameStateMessage(mainMenu, GameStateEventMessage.Start, DestroySongObject);

            songType = songs[0];
            
            
        }

        private void UpdateSong()
        {

            currentSong.UpdateSong();
        }
        private void SetSong()
        {

        }
        public void SetSong(Song songType)
        {
            if(songType == null) {
                Debug.LogError("songType is null.");
                songType = null;
                return;
            }

            this.songType = songType;
        }
        
        void CreateSongObject()
        {
            if (songType == null)
            {
                Debug.LogError("songType is null.");
                songType = null;
                return;
            }

            currentSong = Instantiate(songType);
            currentSong.StartSong();
            currentSong.name = "Playing Song";
            PlaySong();

        }
        void PlaySong()
        {
            audioSource.Play();
        }
        void PauseSong()
        {
            audioSource.Pause();
        }
        void StopSong()
        {
            audioSource.Stop();
        }
        void DestroySongObject()
        {

            
          //  Debug.Log("Destroy");
            if(currentSong != null)
            {
                currentSong.StopSong();
                Destroy(currentSong.gameObject);
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}