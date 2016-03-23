using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameState : MonoBehaviour {

    
   

    Dictionary<GameStateEventMessage, List<GameStateEvent>> gameStateEvents = null;
   
    float gameStateStartTime;

    GameStateController gameStateController;

    public string GetName() { return name; }
    public float GetStartTime() { return gameStateStartTime; }

    void Start()
    {
       
        InitializeGameStateEvents();
        AddGameStateMessage(GameStateEventMessage.Start, InitializeGameState);
        //  gameStateController = MonoSingleton.GetSingleton("GameState").GetComponent<GameStateController>();
        gameStateController = MSingleton.GetSingleton<GameStateController>();
    }
    void Update()
    {

    }
    private void InitializeGameStateEvents()
    {
        gameStateEvents = new Dictionary<GameStateEventMessage, List<GameStateEvent>>();
        foreach (GameStateEventMessage message in Enum.GetValues(typeof(GameStateEventMessage)))
        {
            // Debug.Log("Hi" + message.ToString() + gameStateName);
            gameStateEvents.Add(message, new List<GameStateEvent>());
        }
    }
    public void SendMessageToGameStateEvents(GameStateEventMessage message)
    {
        if(gameStateEvents == null)
        {
            InitializeGameStateEvents();
        }
        List<GameStateEvent> selectedEvents;
        gameStateEvents.TryGetValue(message, out selectedEvents);
        if(selectedEvents == null)
        {
            Debug.LogError("Message returned null List<GameStateEvent>");
        }

        foreach(GameStateEvent gse in selectedEvents)
        {
            gse();
        }
        
    }

    public void AddGameStateMessage(GameStateEventMessage message, GameStateEvent gameStateEvent)
    {
        
        if (gameStateEvents == null)
        {
            InitializeGameStateEvents();
        }
        /*
        List<GameStateEvent> selectedEvents = new List<GameStateEvent>();
        gameStateEvents.TryGetValue(message, out selectedEvents);
        if (selectedEvents == null)
        {
            Debug.LogError("Message returned null List<GameStateEvent>");
        }
        selectedEvents.Add(gameStateEvent);
        gameStateEvents[message] = selectedEvents;
        */
        gameStateEvents[message].Add(gameStateEvent);
        if(gameStateController == null)
        {
            gameStateController = MSingleton.GetSingleton<GameStateController>();//MonoSingleton.GetSingleton("GameState").GetComponent<GameStateController>();
        }
        if(message == GameStateEventMessage.Start)
        {
            if(gameStateController.GetActiveGameState() != null)
            {
                if(gameStateController.GetActiveGameState() == this)
                {
                    gameStateEvent();
                }
            }
            else if (gameStateController.initialGameState == this)
            {
                gameStateEvent();
            }
           // Debug.Log("Called " + gameStateEvent + " " + name);
        }

    }
    
    private void InitializeGameState()
    {
        gameStateStartTime = Time.time;
    }
}
