using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Song;

public class GameScore : MonoBehaviour {

    public Text scoreValueText;
    public Text scoreMultiplierText;
    public Text scoreStreakText;

    float currentScore = 0;
    SongDifficulty songDifficulty;
    int currentNoteStreak = 0;
    const int noteStreakInterval = 5;
    const float noteStreakMultiplierAdd = .25f;
    const float noteTimeDecreaseInterval = .05f;
    const float noteScore = 10;
    public void NoteClick()
    {
        currentNoteStreak += 1;
        if(currentNoteStreak % 5 == 0)
        {
           // MSingleton.GetSingleton<Song>().SetDesiredNoteTime()
        }
        currentScore += noteScore * GetMultiplier();
        SetText();
    }
    public void NoteFail()
    {
        currentNoteStreak = 0;
        SetText();
    }
    public void SetDifficultyMultiplier(SongDifficulty songDifficulty)
    {
        this.songDifficulty = songDifficulty;
        
    }
    private float GetMultiplier()
    {
        float multiplier = /* songDifficulty.GetScoreMultilier() + */ (Mathf.Floor(currentNoteStreak / noteStreakInterval) * noteStreakMultiplierAdd);
        return multiplier;
    }

    private void SetText()
    {
        SetText(scoreMultiplierText, GetMultiplier().ToString());
        SetText(scoreValueText, currentScore.ToString());
        SetText(scoreStreakText, currentNoteStreak.ToString());
    }
    private void SetText(Text t, string val)
    {
        if (t)
        {
            t.text = val;

        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
