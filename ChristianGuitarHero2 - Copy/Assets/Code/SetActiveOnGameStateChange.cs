using UnityEngine;
using System.Collections;

public class SetActiveOnGameStateChange : MonoBehaviour {

    public GameStateEventMessage message;
    public GameObject[] objectsToSet;
    public bool value;
    public GameState gameState;

    // Use this for initialization
    void Start()
    {
        gameState.AddGameStateMessage(message, SetOnChange);
    }

    void SetOnChange()
    {
        foreach (GameObject go in objectsToSet)
        {
            go.SetActive(value);
        }
    }
}
