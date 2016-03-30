using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Song
{
    public class SongListUI : MonoBehaviour
    {
        //Distance between each button
        [SerializeField]
        private Color selectedColor;

        [SerializeField]
        private Color unselectedColor;

        [SerializeField]
        private float selectedScale = 1f;

        [SerializeField]
        private float unselectedScale = 1.2f;

        [SerializeField]
        private float buttonDistance = 100;

        //all buttons are put under this gameobject
        [SerializeField]
        private Transform buttonParent;

        [SerializeField]
        ToggleGroup toggleGroup;

        //used to create other buttons
        [SerializeField]
        private GameObject buttonBase;

        private const string songTitleObjectName = "Title";
        private const string songArtistObjectName = "Artist";

        SongController songController;
        GameState selectDifficultyGameState;
        GameState songSelectGameState;
        GameStateController gameStateController;
        Song[] songs;
        GameObject[] guiObjects;
        AudioSource previewSource;
        GameObject prevSelectedObject;

        // Use this for initialization
        void Start()
        {
            previewSource = GetComponent<AudioSource>();
            //songController = MonoSingleton.GetSingleton("Song").GetComponent<SongController>();
            songController = MSingleton.GetSingleton<SongController>();
            //gameStateController = MonoSingleton.GetSingleton("GameState").GetComponent<GameStateController>();

            gameStateController = MSingleton.GetSingleton<GameStateController>();

            selectDifficultyGameState = gameStateController.GetGameState("Difficulty Select");
            selectDifficultyGameState.AddGameStateMessage(GameStateEventMessage.Start, new GameStateEventData(this.gameObject, SetSong));

            songSelectGameState = gameStateController.GetGameState("Song List");
            songSelectGameState.AddGameStateMessage(GameStateEventMessage.Start, new GameStateEventData(this.gameObject, SetDefaultSong));
            songSelectGameState.AddGameStateMessage(GameStateEventMessage.Exit, new GameStateEventData(this.gameObject, StopPreview));

            songs = songController.songs;
            guiObjects = new GameObject[songs.Length];
            

            for (int i = 0; i < songs.Length; i++)
            {
                guiObjects[i] = CreateButton(songs[i]);
                guiObjects[i].GetComponent<Toggle>().isOn = false;
            }
            
            Debug.Log(songs.Length);
        }
        private void SetDefaultSong()
        {
            guiObjects[0].GetComponent<Toggle>().isOn = true;
            prevSelectedObject = GetSelectedGUIObject();
            SetSong();
            SetPreviewClip(songs[0]);
        }
        private GameObject CreateButton(Song songResource)
        {
            GameObject newButton = Instantiate(buttonBase);

            newButton.transform.SetParent(buttonParent, false);
            newButton.name = "Awesome Button " + songResource.GetName();
            RectTransform newRectTransform = newButton.GetComponent<RectTransform>();

            float buttonHeight;
            buttonHeight = ((songResource.GetUIOrder() ) * -buttonDistance);
            buttonHeight -= ((newRectTransform.sizeDelta.y) * .5f);

            newRectTransform.anchoredPosition = new Vector2(0, buttonHeight);
            Text titleText = newButton.transform.Find(songTitleObjectName).gameObject.GetComponent<Text>();
            Text artistText = newButton.transform.Find(songArtistObjectName).gameObject.GetComponent<Text>();

            titleText.text = songResource.GetName();
            artistText.text = songResource.GetArtist();

            newButton.GetComponent<Toggle>().group = toggleGroup;

            return newButton;

            //button listener code taken from
            //http://answers.unity3d.com/questions/791573/46-ui-how-to-apply-onclick-handler-for-button-gene.html


            //  AddSongListener(button, songResource); // Using the iterator as argument will capture the variable
            //  AddStateChangeListener(button);
        }
        /*
        private void AddStateChangeListener(Button b)
        {
            b.onClick.AddListener(() => gameStateController.SetGameState(selectDifficultyGameState));
        }
        private void AddSongListener(Button b, Song songResource)
        {
            
            b.onClick.AddListener(() => songController.SetSong(songResource));
        }
        */
        void Update()
        {
            /*
            for (int i = 0; i < guiObjects.Length; i++)
            {
                if (guiObjects[i].GetComponent<Toggle>().isOn)
                {
                    return guiObjects[i];
                }
            }
            */

            for (int i = 0; i < guiObjects.Length; i++)
            {
                if (guiObjects[i].GetComponent<Toggle>().isOn)
                {
                    if (guiObjects[i] != prevSelectedObject)
                    {
                        SetPreviewClip(songs[i]);
                       
                    }
                }
            }

            prevSelectedObject = GetSelectedGUIObject();
        }
        private void SetPreviewClip(Song s)
        {
            previewSource.Stop();
            previewSource.clip = s.GetSongClip();
            previewSource.time = s.GetPreviewStartTime();
            previewSource.Play();
        }
        private GameObject GetSelectedGUIObject()
        {
            for (int i = 0; i < guiObjects.Length; i++)
            {
                if(guiObjects[i].GetComponent<Toggle>().isOn)
                {
                    return guiObjects[i];
                }
            }
            return guiObjects[0];
        }
        private void SetSong()
        {
            
            Toggle[] toggles = buttonParent.GetComponentsInChildren<Toggle>();
            for(int i = 0; i < toggles.Length; i++)
            {
                if(toggles[i].isOn)
                {
                    songController.SetSongType(songs[i]);
                    return;
                }
            }
            songController.SetSongType(songs[0]);
        }
        private void StopPreview()
        {
            previewSource.clip = null;
            previewSource.Stop();
        }
    }
}