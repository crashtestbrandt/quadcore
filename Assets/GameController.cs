using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public float ResetDelaySeconds = 3.0f; 
    // Start is called before the first frame update
    void Start()
    {
        FloorController.BallCollidedWithFloorEvent += OnReset;
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
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            GameObject.Destroy(ball);
        }

        player.StartTurn();

    }

    
}
