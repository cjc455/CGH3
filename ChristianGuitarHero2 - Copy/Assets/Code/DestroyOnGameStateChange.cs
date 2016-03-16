﻿using UnityEngine;
using System.Collections;

public class DestroyOnGameStateChange : MonoBehaviour {

    public GameStateEventMessage message;
    public GameObject[] objectsToDestroy;
    public GameState gameState;

	// Use this for initialization
	void Start () {
        gameState.AddGameStateMessage(message, DestroyOnChange);
	}
	
	void DestroyOnChange()
    {
        foreach(GameObject go in objectsToDestroy)
        {
            Destroy(go);
        }
    }
}
