using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameModeType
    {
        LOCAL_MULTIPLAYER,
        LOCAL_AI,
        NETWORK_MULTIPLAYER,
        AI_VS_AI
    }
    GameModeType gameMode;
    public bool AllowGameModeOverride = false;
    public GameModeType GameModeOverride = GameModeType.LOCAL_MULTIPLAYER;

    public GameObject PlayerPrefab;
    public GameObject AIPlayerPrefab;
    public GameObject BallPrefab;
    public GameObject InfoUI;

    public Text DebugText;
    public Text DebugLabel1;
    public Text DebugLabel2;
    public Slider DebugSlider;

    public static float YSpeedFactor = 1.0f;

    GameObject[] players;
    int currentPlayer;

    public static bool Quitting { get; set; }
    public static bool GameOver { get; set; }

    public float ResetDelaySeconds = 3.0f;

    void Start()
    {
        if (AllowGameModeOverride)
        {
            gameMode = GameModeOverride;
        }
        else
        {
            gameMode = GameState.GameMode;
        }

        Debug.Log("Starting game in mode: " + gameMode);
        // Register callbacks
        FloorController.BallCollidedWithFloorEvent += OnResetTurn;
        BallController.ThrowTimer += OnResetTurn;
        BallGrabber.BallGrabbedByCell += OnResetRequestedByBoard;
        BoardController.GameOver += OnGameOver;
        if (DebugSlider)
        {
            YSpeedFactor = DebugSlider.value / 10.0f;
            if (DebugLabel1)
            {
                DebugLabel1.text = YSpeedFactor.ToString();
            }
            DebugSlider.onValueChanged.AddListener(delegate {OnSetDebugVelocity();});
        }

        //OnResetTurn();
        StartNewGame();
    }

    void StartNewGame()
    {
        Quitting = false;
        GameOver = false;
        InfoUI.SetActive(false);

        players = new GameObject[2];
        currentPlayer = UnityEngine.Random.Range(1,3);
        OnResetTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void OnResetTurn()
    {
        Debug.Log("Reset requested! Waiting " + ResetDelaySeconds + " seconds ...");
        await Task.Delay(TimeSpan.FromSeconds(ResetDelaySeconds));

        if (Quitting || GameOver) return;

        Debug.Log("Resetting.");

        // If a turn just ended
        if (players[currentPlayer-1] != null)
        {
            // Make current player inactive
            players[currentPlayer-1].SetActive(false);

            // Change to next player
            currentPlayer = currentPlayer == 1? 2 : 1;
            Debug.Log("Switching to player " + currentPlayer + " ...");

        }

        // If this is the player's first turn
        if (players[currentPlayer-1] == null)
        {
            Debug.Log("Attempting to create player " + currentPlayer + " ...");
            switch (gameMode)
            {
                case GameModeType.LOCAL_MULTIPLAYER:
                    players[currentPlayer-1] = Instantiate(PlayerPrefab, this.transform.position, Quaternion.identity);
                    break;
                case GameModeType.LOCAL_AI:
                    if (currentPlayer == 1)
                        players[currentPlayer-1] = Instantiate(PlayerPrefab, this.transform.position, Quaternion.identity);
                    else
                        players[currentPlayer-1] = Instantiate(AIPlayerPrefab, this.transform.position, Quaternion.identity);
                    break;
                case GameModeType.NETWORK_MULTIPLAYER:
                    break;
                case GameModeType.AI_VS_AI:
                    players[currentPlayer-1] = Instantiate(AIPlayerPrefab, this.transform.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
            players[currentPlayer-1].GetComponent<IPlayerController>().SetPlayerNumber(currentPlayer);
            players[currentPlayer-1].GetComponent<IPlayerController>().SetDebugTextObject((DebugText != null)? DebugText : null);
        }

        // Make sure this player is active, whether newly created or not
        players[currentPlayer-1].SetActive(true);

        Debug.Log("Attempting to start turn for player " + currentPlayer + " ...");
        players[currentPlayer-1].GetComponent<IPlayerController>().StartTurn();

    }

    void OnResetRequestedByBoard(int x, int y, GameObject ball)
    {
        if (!GameOver && !Quitting) OnResetTurn();
    }

    void OnGameOver(GameObject ball)
    {
        GameOver = true;

        InfoUI.SetActive(true);
        InfoUI.GetComponentInChildren<Text>().text = ((ball.tag == "Player1")? "PLAYER 1" : "PLAYER 2") + " WINS";
        if (players != null)
        {
            foreach (GameObject player in players)
            {
                if (player != null) Destroy(player);
            }
        }

        Debug.Log("Game over! Waiting ...");
        //await Task.Delay(TimeSpan.FromSeconds(ResetDelaySeconds));

        if (Quitting) return;
    }

    public void OnNewGame()
    {
        Debug.Log("New game requested!");
        GameOver = false;
        
        if (!Quitting) StartNewGame();
    }

    private void OnDestroy() {
        foreach (GameObject player in players)
        {
            if (player != null) Destroy(player);
        }
        Quitting = true;
        
        Destroy(this);
    }

    public void ReturnToMainMenu()
    {
        FloorController.BallCollidedWithFloorEvent -= OnResetTurn;
        BallController.ThrowTimer -= OnResetTurn;
        BallGrabber.BallGrabbedByCell -= OnResetRequestedByBoard;
        BoardController.GameOver -= OnGameOver;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void OnSetDebugVelocity()
    {
        DebugSlider.onValueChanged.RemoveAllListeners();
        if (DebugSlider != null)
        {
            YSpeedFactor = DebugSlider.value / 10.0f;
            
        }
        if (DebugLabel1 != null)
        {
            DebugLabel1.text = YSpeedFactor.ToString();
        }
        DebugSlider.onValueChanged.AddListener(delegate {OnSetDebugVelocity();});
    }
    

}
