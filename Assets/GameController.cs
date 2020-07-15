using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class GameController : MonoBehaviour
{
    //public PlayerController player; // TODO: Remove

    public GameObject PlayerPrefab;
    public GameObject BallPrefab;
    GameObject[] players;
    int currentPlayer;

    public float ResetDelaySeconds = 3.0f; 
    // Start is called before the first frame update
    void Start()
    {
        players = new GameObject[2];
        currentPlayer = UnityEngine.Random.Range(1,3);
        FloorController.BallCollidedWithFloorEvent += OnReset;

        OnReset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void OnReset()
    {
        Debug.Log("Reset requested! Waiting " + ResetDelaySeconds + " seconds ...");
        await Task.Delay(TimeSpan.FromSeconds(ResetDelaySeconds));
        Debug.Log("Resetting.");
        /*
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            GameObject.Destroy(ball);
        }
        */

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
        }

        // Make sure this player is active, whether newly created or not
        players[currentPlayer-1].SetActive(true);

        Debug.Log("Attempting to start turn for player " + currentPlayer + " ...");
        players[currentPlayer-1].GetComponent<PlayerController>().StartTurn();

    }

    private void OnDestroy() {
        foreach (GameObject player in players)
        {
            Destroy(player);
        }
    }
}
