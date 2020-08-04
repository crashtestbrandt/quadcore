using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class RemotePlayerController : MonoBehaviour, IPlayerController
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
        Debug.Log("Created a remote player.");
        mainCamera = Camera.main;

        
    }

    void FixedUpdate()
    {
    }

    public async void StartTurn()
    {
        Debug.Log("Remote player starting turn");
        if (debugText != null)
        {
            debugText.text = "Turn: Player " + playerNumber + " (remote)";
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
        Vector3 launchVelocity = Vector3.zero;

        orb.GetComponentInChildren<Collider>().gameObject.tag = "Player" + playerNumber.ToString();
        await Task.Delay(TimeSpan.FromSeconds(5.0f));
        if (debugText != null)
        {
            debugText.text += "\n\nOrb launching with velocity: " + launchVelocity;
        }
        orb.GetComponent<BallController>().Launch(launchVelocity);
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
