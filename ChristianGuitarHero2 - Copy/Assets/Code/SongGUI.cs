using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Song;

public class SongGUI : MonoBehaviour {

    [SerializeField]
    Text[] activeSongTitle;

    [SerializeField]
    Text[] activeSongArtist;

    static SongController songController;

    // Use this for initialization
    void Start () {
        GameState songSelectionState = MSingleton.GetSingleton<GameStateController>().GetGameState("Song List");
        songSelectionState.AddGameStateMessage(GameStateEventMessage.Exit, new GameStateEventData(this.gameObject, SetText));

        songController = MSingleton.GetSingleton<SongController>();
    }
	void SetText()
    {
        if (songController.GetActiveSongType() != null)
        {
            
            foreach (Text t in activeSongTitle)
            {

                t.text = songController.GetActiveSongType().GetName();
            }
            foreach (Text t in activeSongArtist)
            {
                t.text = songController.GetActiveSongType().GetArtist();
            }
        }
        else
        {
            Debug.Log("Song null");
        }
    }
	// Update is called once per frame
	void Update () {
	   
	}
}
