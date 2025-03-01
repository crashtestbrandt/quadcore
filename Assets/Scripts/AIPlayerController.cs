﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class AIPlayerController : MonoBehaviour, IPlayerController
{
    Camera mainCamera;

    GameObject orb;
    GameObject ChosenOrbPrefab;
    public GameObject[] OrbPrefabs;

    public float BallZOffset = 0.5f;
    public float BallYOffset = 1.3f;
    

    int playerNumber;
    Text debugText = null;

    void Awake()
    {
        Debug.Log("Created an AI player.");
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
    }

    public async void StartTurn()
    {
        Debug.Log("AI player starting turn");
        if (debugText != null)
        {
            debugText.text = "Turn: Player " + playerNumber + " (AI)";
        }
        ChosenOrbPrefab = OrbPrefabs[playerNumber-1];
        orb = Instantiate(
            ChosenOrbPrefab,
            new Vector3(
                transform.position.x,
                transform.position.y + BallYOffset,
                transform.position.z + mainCamera.nearClipPlane + BallZOffset
            ),
            Quaternion.AngleAxis(45.0f, transform.right)
        );
        // Get the board
        // Determine which column to target
        // Get target position
        Vector3 target = new Vector3(0.0f, 1.54f, 1.88f);
        target = target - orb.transform.position;
        
        float[] skews = new float[7];
        for (int i = 0; i < 7; i++)
        {
            skews[i] = 0.3f * ((float)i-3.0f);
        }
        Vector3 velocity = new Vector3(skews[UnityEngine.Random.Range(0,7)], 3.5f, 3.5f);

        orb.GetComponentInChildren<Collider>().gameObject.tag = "Player" + playerNumber.ToString();
        await Task.Delay(TimeSpan.FromSeconds(5.0f));
        if (debugText != null)
        {
            debugText.text += "\n\nOrb launching with velocity: " + velocity;
        }
        orb.GetComponent<BallController>().Launch(velocity);
    }

    public void SetPlayerNumber(int num)
    {
        playerNumber = num;
    }

    private void OnDisable() {
        if (orb != null) Destroy(orb);
    }

    public void SetDebugTextObject(Text textObject)
    {
        debugText = textObject;
    }

}
