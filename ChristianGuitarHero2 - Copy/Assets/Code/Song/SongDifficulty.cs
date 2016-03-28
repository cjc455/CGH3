using UnityEngine;
using System.Collections;

public class SongDifficulty : MonoBehaviour {

    [SerializeField]
    string difficultyName;

    [SerializeField]
    float noteTime;

    [SerializeField]
    float scoreMultiplier;

    [SerializeField]
    float noteTimeIncreaseRate = 1;
    
    public string GetDifficultyName()
    {
        return difficultyName;
    }
    public float GetScoreMultilier()
    {
        return scoreMultiplier;
    }
    public float GetNoteTime()
    {
        return noteTime;
    }
    public float GetNoteTimeIncreaseRate()
    {
        return noteTimeIncreaseRate;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
