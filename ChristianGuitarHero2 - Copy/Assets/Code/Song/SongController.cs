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
   //     [SerializeField]
     //   float noteTime = .5f;
        [SerializeField]
        float noteClickRange = .5f;
        [SerializeField]
        float noteFailRange = 2f;
        [SerializeField]
        float noteFailScoreChange = -1;
        [SerializeField]
        float noteClickScoreChange = 1;
        [SerializeField]
        float noteTransparencyFadeTime = 2;
        [SerializeField]
        float noteYTransitionEnd = 0;
        [SerializeField]
        float noteViewportClickRange = 25;

        public float GetNoteViewportClickRange() { return noteViewportClickRange; }
        AudioSource audioSource;

        private Song currentSong;
        private SongDifficulty songDifficulty;
        [SerializeField]
        Song songType;

        public float GetNoteTime(){ return songDifficulty.GetNoteTime(); }
        public float GetNoteClickRange() { return noteClickRange;}

        public float GetNoteFailScoreChange() { return noteFailScoreChange; }
        public float GetNoteClickScoreChange() { return noteClickScoreChange; }
        public float GetNoteTransparenctFadeTime() { return noteTransparencyFadeTime; }
        public float GetNoteFailRange() { return noteFailRange; }
        public float GetNoteYTransitionEnd() { return noteYTransitionEnd;  }
        public Song GetActiveSong()
        {
            return currentSong;
        }
        public Song GetActiveSongType()
        {
            return songType;
        }
        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            GameStateController gameStateController = MSingleton.GetSingleton<GameStateController>();//MonoSingleton.GetSingleton("GameState").GetComponent<GameStateController>();
            GameState playing = gameStateController.GetGameState("Playing");
            GameState mainMenu = gameStateController.GetGameState("Main Menu");
            GameState pause = gameStateController.GetGameState("Pause");
            GameState songList = gameStateController.GetGameState("Song List");
            GameState songFinish = gameStateController.GetGameState("Song Finish");
            GameState difficultyState = gameStateController.GetGameState("Difficulty Select");

            difficultyState.AddGameStateMessage(GameStateEventMessage.Exit, new GameStateEventData(this.gameObject, CreateSongObject));
            playing.AddGameStateMessage(GameStateEventMessage.Update, new GameStateEventData(this.gameObject, UpdateSong));
          //  playing.AddGameStateMessage(GameStateEventMessage.Exit, new GameStateEventData(this.gameObject, EndSong));

            
            mainMenu.AddGameStateMessage(GameStateEventMessage.Start, new GameStateEventData(this.gameObject, DestroySongObject));
            songList.AddGameStateMessage(GameStateEventMessage.Start, new GameStateEventData(this.gameObject, DestroySongObject));

            pause.AddGameStateMessage(GameStateEventMessage.Start, new GameStateEventData(this.gameObject, PauseSong));
            pause.AddGameStateMessage(GameStateEventMessage.Exit, new GameStateEventData(this.gameObject, UnpauseSong));

            songFinish.AddGameStateMessage(GameStateEventMessage.Start, new GameStateEventData(this.gameObject, StopSong));
            songType = songs[0];
            
            
        }
        private void EndSong()
        {

        }
        private void UpdateSong()
        {

            currentSong.UpdateSong();
        }

        public void SetSongType(Song songType)
        {
            if(songType == null) {
                Debug.LogError("songType is null.");
                songType = null;
                return;
            }

            this.songType = songType;
        }
        public void SetSongDifficulty(SongDifficulty songDifficulty)
        {
            this.songDifficulty = songDifficulty;
        }
        public void ActivateSong()
        {
            CreateSongObject();
        }
        void CreateSongObject()
        {
            if (songType == null)
            {
                Debug.LogError("songType is null.");
                songType = null;
                return;
            }
            if(currentSong != null)
            {
                StopSong();
                DestroySongObject();
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
        void UnpauseSong()
        {
            PlaySong();
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
            StopSong();
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}