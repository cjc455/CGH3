using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Song;

public class GameVariables : MonoBehaviour {


    public bool debugMode = true;

    private float score;
    int noteStreak;

    public void UpdateScoreNoteClick()
    {
        noteStreak += 1;
       
    }
    public void UpdateScoreNoteFail()
    {
        noteStreak = 0;
    }
    public void UpdateScore(float valueAdd)
    {
        score += valueAdd;
    }
    public float GetScore() { return score; }
    public void DeMess(string message)
    {
        if(debugMode) { Debug.Log(message); }
    }
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
