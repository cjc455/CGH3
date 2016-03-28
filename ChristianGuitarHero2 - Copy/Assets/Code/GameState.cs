using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateEventData
{
    public GameObject gameObject;
    public GameStateEvent gameStateEvent;
    
    public GameStateEventData(GameObject gameObject, GameStateEvent gameStateEvent)
    {
        this.gameStateEvent = gameStateEvent;
        this.gameObject = gameObject;
    }
}

public class GameState : MonoBehaviour {

    
   

    Dictionary<GameStateEventMessage, List<GameStateEventData>> gameStateEvents = null;
   
    float gameStateStartTime;

    GameStateController gameStateController;

    public string GetName() { return name; }
    public float GetStartTime() { return gameStateStartTime; }

    void Start()
    {
       
        InitializeGameStateEvents();
        AddGameStateMessage(GameStateEventMessage.Start, new GameStateEventData(this.gameObject, InitializeGameState));
        //  gameStateController = MonoSingleton.GetSingleton("GameState").GetComponent<GameStateController>();
        gameStateController = MSingleton.GetSingleton<GameStateController>();
    }
    void Update()
    {

    }
    private void InitializeGameStateEvents()
    {
        gameStateEvents = new Dictionary<GameStateEventMessage, List<GameStateEventData>>();
        foreach (GameStateEventMessage message in Enum.GetValues(typeof(GameStateEventMessage)))
        {
            // Debug.Log("Hi" + message.ToString() + gameStateName);
            gameStateEvents.Add(message, new List<GameStateEventData>());
        }
    }
    public void SendMessageToGameStateEvents(GameStateEventMessage message)
    {
        if(gameStateEvents == null)
        {
            InitializeGameStateEvents();
        }
        List<GameStateEventData> selectedEvents;
        gameStateEvents.TryGetValue(message, out selectedEvents);
        if(selectedEvents == null)
        {
            Debug.LogError("Message returned null List<GameStateEvent>");
        }
        for(int i = 0; i < selectedEvents.Count; i++)
        {
            if (selectedEvents[i].gameObject == null)
            {
                selectedEvents.RemoveAt(i);

            }
            else
            {
                selectedEvents[i].gameStateEvent();
            }
        }
       
        
    }

    public void AddGameStateMessage(GameStateEventMessage message, GameStateEventData gameStateEventData)
    {
        
        if (gameStateEvents == null)
        {
            InitializeGameStateEvents();
        }
        
        gameStateEvents[message].Add(gameStateEventData);
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
                    gameStateEventData.gameStateEvent();
                }
            }
            else if (gameStateController.initialGameState == this)
            {
                gameStateEventData.gameStateEvent();
            }
           // Debug.Log("Called " + gameStateEvent + " " + name);
        }

    }
    
    private void InitializeGameState()
    {
        gameStateStartTime = Time.time;
    }
}
