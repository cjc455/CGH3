using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void GameStateEvent();

public enum GameStateEventMessage
{
    Start,
    Exit,
    Update,
    UpdateSleep
}

public class GameStateController : MonoBehaviour {

    //call gamestateevents in this class
    //set the variables in gameState in the GameState class

        /*
            Problem: The Start() actions are called before all of the foreign objects have addded their values to the list of actions,
            for the initial gameState.



    */
    public GameState initialGameState;
    GameState activeGameState = null;
    GameState previousGameState = null;
    GameState[] gameStates;
    GameVariables gameVars;

	// Use this for initialization
	void Start () {
        gameVars = MSingleton.GetSingleton<GameVariables>();//MonoSingleton.GetSingleton("GameVariables").GetComponent<GameVariables>();
        gameStates = GetComponentsInChildren<GameState>();
        SetGameState(initialGameState);

    }
	
	// Update is called once per frame
	void Update () {
        activeGameState.SendMessageToGameStateEvents(GameStateEventMessage.Update, previousGameState);
       // Debug.Log(GetActiveGameState().GetName());
        foreach(GameState state in gameStates)
        {
            if(state != activeGameState)
            {
                state.SendMessageToGameStateEvents(GameStateEventMessage.UpdateSleep, previousGameState);
            }
        }
    }
    public GameState GetActiveGameState()
    {
        return activeGameState;
    }

    public GameState GetGameState(string gameStateName)
    {
        GameState newGameState = Array.Find(gameStates, s => s.GetName() == gameStateName);
        if(newGameState == null)
        {
            Debug.LogError("Could not find gamestate " + gameStateName);
            return null;
        }
        return newGameState;
    }
    
    public void AddGameStateMessage(GameState[] gameStates, GameStateEventMessage message, GameStateEventData gameStateEventData)
    {
        foreach(GameState s in gameStates)
        {
            s.AddGameStateMessage(message, gameStateEventData);
        }
        
    }
    public void SetGameState(GameState newGameState)
    {
        SetGameState(newGameState.GetName());
    }
    public void SetGameState(string gameStateName)
    {
        //Send message to "old" gamestate
        if(activeGameState != null)
        {
            activeGameState.SendMessageToGameStateEvents(GameStateEventMessage.Exit, previousGameState);
        }

        GameState newGameState = Array.Find(gameStates, s => s.GetName() == gameStateName);
        if(newGameState == null)
        {
            gameVars.DeMess("Game state " + gameStateName + " does not exist.");
            return;
            //I don't want this here, because it could create invalid gamestates, increasing anxiety for errors
           // newGameStateData = new GameStateData(gameStateName);
            
        }
        activeGameState = newGameState;
        activeGameState.SendMessageToGameStateEvents(GameStateEventMessage.Start, previousGameState);

        previousGameState = activeGameState;
        
        
        
    }
   

}
