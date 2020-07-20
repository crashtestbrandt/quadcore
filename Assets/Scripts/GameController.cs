using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject BallPrefab;
    public GameObject InfoUI;

    GameObject[] players;
    int currentPlayer;

    public static bool Quitting { get; set; } = false;

    public float ResetDelaySeconds = 3.0f;

    void Start()
    {
        InfoUI.SetActive(false);

        players = new GameObject[2];
        currentPlayer = UnityEngine.Random.Range(1,3);

        // Register callbacks
        FloorController.BallCollidedWithFloorEvent += OnResetTurn;
        BallGrabber.BallGrabbedByCell += OnResetRequestedByBoard;
        BoardController.GameOver += OnGameOver;

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

        if (Quitting) return;

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
            players[currentPlayer-1] = Instantiate(PlayerPrefab, new Vector3(0,0,2), Quaternion.identity);
            players[currentPlayer-1].GetComponent<PlayerController>().PlayerNumber = currentPlayer;
        }

        // Make sure this player is active, whether newly created or not
        players[currentPlayer-1].SetActive(true);

        Debug.Log("Attempting to start turn for player " + currentPlayer + " ...");
        players[currentPlayer-1].GetComponent<PlayerController>().StartTurn();

    }

    void OnResetRequestedByBoard(int x, int y, string tag)
    {
        OnResetTurn();
    }

    void OnGameOver(string tag)
    {
        InfoUI.SetActive(true);
        InfoUI.GetComponentInChildren<Text>().text = ((tag == "Player1")? "PLAYER 1" : "PLAYER 2") + " WINS";
        

        Debug.Log("Game over! Waiting ...");
        //await Task.Delay(TimeSpan.FromSeconds(ResetDelaySeconds));

        if (Quitting) return;
    }

    public void OnNewGame()
    {
        Debug.Log("New game requested!");
    }

    private void OnDestroy() {
        foreach (GameObject player in players)
        {
            if (player != null) Destroy(player);
        }
        Quitting = true;
        Destroy(this);
    }

}
