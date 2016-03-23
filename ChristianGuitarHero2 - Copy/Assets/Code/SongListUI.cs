using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Song
{
    public class SongListUI : MonoBehaviour
    {
        //Distance between each button
        [SerializeField]
        private float buttonDistance = 100;

        //all buttons are put under this gameobject
        [SerializeField]
        private Transform buttonParent;

        private const string songTitleObjectName = "Title";
        private const string songArtistObjectName = "Artist";
        private const string songDifficultyName = "Difficulty";
        private const string buttonName = "Button";

        //used to create other buttons
        [SerializeField]
        private GameObject buttonBase;

        //Set the height to the sum of all the heights of all the buttons
        [SerializeField]
        private RectTransform contentRect;

        [SerializeField]
        ScrollSnapRect snapRect;

        SongController songController;
        GameState playingGameState;
        GameStateController gameStateController;

        // Use this for initialization
        void Start()
        {

            //songController = MonoSingleton.GetSingleton("Song").GetComponent<SongController>();
            songController = MSingleton.GetSingleton<SongController>();
            //gameStateController = MonoSingleton.GetSingleton("GameState").GetComponent<GameStateController>();

            gameStateController = MSingleton.GetSingleton<GameStateController>();

            playingGameState = gameStateController.GetGameState("Playing");


            Song[] songs = songController.songs;
            Button[] buttons = new Button[songs.Length];

            
            foreach (Song s in songs)
            {
                CreateButton(s);
            }

            if (snapRect != null)
                snapRect.Initialize();
            if (contentRect != null)
                contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, songs.Length * buttonDistance);
            Debug.Log(songs.Length);
        }
        private void CreateButton(Song songResource)
        {
            GameObject newButton = Instantiate(buttonBase);

            newButton.transform.SetParent(buttonParent, false);
            newButton.name = "Awesome Button " + songResource.GetName();
            RectTransform newRectTransform = newButton.GetComponent<RectTransform>();
            //newRectTransform.offsetMin = new Vector2(0, songResource.GetSongUIOrder() * buttonDistance);

            float buttonHeight = 0;

            buttonHeight = ((songResource.GetUIOrder() ) * -buttonDistance);
            //Subtract half of the height as an absolute offset so the button is centered properly
            buttonHeight -= ((newRectTransform.sizeDelta.y) * .5f);

            newRectTransform.anchoredPosition = new Vector2(0, buttonHeight);

            Text titleText = newButton.transform.Find(songTitleObjectName).gameObject.GetComponent<Text>();
            Text artistText = newButton.transform.Find(songArtistObjectName).gameObject.GetComponent<Text>();
            Text difficultyText = newButton.transform.Find(songDifficultyName).gameObject.GetComponent<Text>();
            Button button = newButton.transform.Find(buttonName).gameObject.GetComponent<Button>();

            titleText.text = songResource.GetName();
            artistText.text = songResource.GetArtist();
            difficultyText.text = songResource.GetDifficulty().ToString();

            //button listener code taken from
            //http://answers.unity3d.com/questions/791573/46-ui-how-to-apply-onclick-handler-for-button-gene.html

            
            AddSongListener(button, songResource); // Using the iterator as argument will capture the variable
            AddStateChangeListener(button);
        }

        private void AddStateChangeListener(Button b)
        {
            b.onClick.AddListener(() => gameStateController.SetGameState(playingGameState));
        }
        private void AddSongListener(Button b, Song songResource)
        {
            
            b.onClick.AddListener(() => songController.SetSong(songResource));
        }

    }
}