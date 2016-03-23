using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Song;

public class GameVariables : MonoBehaviour {

    public Text scoreText;
    public bool debugMode = true;
    [SerializeField]
    float noteClickScore;

    [SerializeField]
    float noteFailScore;

    [SerializeField]
    float noteMisClickScore;

    [SerializeField]
    float transitionNoteScoreClick;

    [SerializeField]
    int noteStreakCount;

    [SerializeField]
    float noteStreakMultiplier;

    private float currentScore;
    int currentNoteScreak;


    public void UpdateScoreNoteClick(NoteEntry noteEntry)
    {
        currentNoteScreak += 1;
        if (noteEntry.GetNoteType() == NoteEntry.NoteType.Transition)
        {
            UpdateScore(transitionNoteScoreClick);
        }
        else
        {
            UpdateScore(noteClickScore);
        }
    }
    public void UpdateScoreNoteFail(NoteEntry noteEntry)
    {
        currentNoteScreak = 0;
        if (noteEntry.GetNoteType() == NoteEntry.NoteType.Transition)
        {
          //  UpdateScore(transitionNoteScoreClick);
        }
        else
        {
            UpdateScore(noteClickScore);
        }
    }
    private void UpdateScore(float valueAdd)
    {
        currentScore += valueAdd;
        //scoreText.text = currentScore.ToString();
    }
    void ResetScore()
    {
        currentScore = 0;
    }
    public float GetScore() { return currentScore; }
    public void DeMess(string message)
    {
        if(debugMode) { Debug.Log(message); }
    }
    // Use this for initialization
    void Start () {
        //MonoSingleton.GetSingleton("GameState").GetComponent<GameStateController>().GetGameState("Playing").AddGameStateMessage(GameStateEventMessage.Start, ResetScore);
       // MSingleton.GetSingleton<GameStateController>
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
