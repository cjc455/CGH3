using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

    public GameState gameState;
    private static GameStateController gameStateController;
    const float speed = 0.5f;
    const float smooth = 0.5f;
    public bool setChildrenActive = true;

    public RectTransform rectTransform;
    public Rect endRect;
    public Rect activeRect;
    public Rect startRect;

    private Rect desiredRect;

	// Use this for initialization
	void Start () {
        if(gameStateController == null)
        {
           // gameStateController = MonoSingleton.GetSingleton("GameState").GetComponent<GameStateController>();
            gameStateController = MSingleton.GetSingleton<GameStateController>();
        }
        EndMenu();
        gameState.AddGameStateMessage(GameStateEventMessage.Start, new GameStateEventData(this.gameObject, StartMenu));
        gameState.AddGameStateMessage(GameStateEventMessage.Exit, new GameStateEventData(this.gameObject, EndMenu));
        if(!rectTransform)
        {
            rectTransform = GetComponent<RectTransform>();
        }

    }
	
	// Update is called once per frame
	void Update () {
        UpdateMenu();
       // Debug.Log("Hi");
       /*
        if(gameState.GetName() == gameStateController.GetActiveGameState().GetName())
        {
            Debug.Log("Hi" + gameState.GetName());
            SetChildrenActive(true);
        }
        else
        {
            Debug.Log("yo" + gameState.GetName() );
            SetChildrenActive(false);
        }
        */
    }
    void UpdateMenu()
    {
        
       // rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, desiredRect.position, smooth);
    }
    void StartMenu()
    {
        /// desiredRect = activeRect;
        // rectTransform.anchoredPosition = startRect.position;
      //  Debug.Log("Got here");
        if(setChildrenActive)
            SetChildrenActive(true);
    }
    void EndMenu()
    {
        // desiredRect = endRect;
        //Debug.Log("Got here 2");
        if (setChildrenActive)
            SetChildrenActive(false);

    }
    private void SetChildrenActive(bool value)
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform go in children)
        {
            if (go != this.transform)
            {
                 //Debug.Log("Set Active " + go.name + " " + value + " " + gameState.GetName());
                 go.gameObject.SetActive(value);
            }
        }
    }
    private void SetButtonActive(bool value)
    {
        Button[] buttons = rectTransform.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            // b.
        }
    }
}
