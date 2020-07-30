using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIPlayerController : MonoBehaviour, IPlayerController
{
    Camera mainCamera;

    GameObject orb;
    GameObject ChosenOrbPrefab;
    public GameObject[] OrbPrefabs;

    public Text DebugText { get; set; } = null;

    public float BallZOffset = 0.5f;
    public float BallYOffset = 1.3f;
    

    int playerNumber;

    void Awake()
    {
        Debug.Log("Created an AI player.");
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        
    }

    public void StartTurn()
    {
        Debug.Log("AI player starting turn");
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

        // Aim at target position
        Vector3 velocity = new Vector3(
            target.x / Time.fixedDeltaTime,
            (target.y + 0.5f * 9.8f * Mathf.Pow(Time.fixedDeltaTime, 2.0f)) / (Time.fixedDeltaTime * Mathf.Sin(0.125f*Mathf.PI)),
            target.z / (Time.fixedDeltaTime * Mathf.Cos(0.125f * Mathf.PI))
        );

        orb.GetComponentInChildren<BallController>().Launch(velocity);
    }

    public void SetPlayerNumber(int num)
    {
        playerNumber = num;
    }

    private void OnDisable() {
        if (orb != null) Destroy(orb);
    }

}
