using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IPlayerController
{
    const float INCHES_TO_METERS = 39.4f;
    Camera mainCamera;
    bool BallGrabbed = false;

    GameObject orb;
    GameObject ChosenOrbPrefab;
    public GameObject[] OrbPrefabs;

    Text debugText = null;

    public float BallZOffset = 0.5f;
    public float BallYOffset = 1.3f;

    float dpi;
    
    
    Ray ray;

    int playerNumber;

    List<Vector2> screenPositionBuffer;

    void Awake()
    {
        dpi = Screen.dpi;
        Debug.Log("Screen DPI: " + dpi);
        screenPositionBuffer = new List<Vector2>();
        mainCamera = Camera.main;
    }

    public void StartTurn()
    {
        Debug.Log("Local player starting turn");
        if (debugText != null)
        {
            debugText.text = "Turn: Player " + playerNumber + " (local)";
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
        
    }

    void FixedUpdate()
    {
        if (BallGrabbed) WhileBallGrabbed();
    }


    public void OnGrab(InputAction.CallbackContext context)
    {
        ray = mainCamera.ScreenPointToRay(Pointer.current.position.ReadValue());
        RaycastHit hitData;
        if (BallGrabbed)
        {
            if (context.phase == InputActionPhase.Canceled)
            {
                BallGrabbed = false;

                float sumXm = 0.0f;
                float sumYm = 0.0f;
                for (int i = 1; i < screenPositionBuffer.Count; i++)
                {
                    sumXm += (screenPositionBuffer[i].x - screenPositionBuffer[i-1].x) / dpi;
                    sumYm += (screenPositionBuffer[i].y - screenPositionBuffer[i-1].y) / dpi;
                }
                float avgXmps = (sumXm / (float)(screenPositionBuffer.Count)) * (1.0f / Time.fixedDeltaTime) * (1.0f / INCHES_TO_METERS);
                float avgYmps = (sumYm / (float)(screenPositionBuffer.Count)) * (1.0f / Time.fixedDeltaTime) * (1.0f / INCHES_TO_METERS);
                Vector3 launchVelocity = new Vector3(
                    avgXmps,
                    avgYmps,
                    avgYmps
                    );
                launchVelocity = GameController.SpeedFactor * launchVelocity;
                if (debugText != null)
                {
                    debugText.text += "\n\nOrb launching with velocity: " + launchVelocity;
                }
                
                orb.GetComponentInChildren<Collider>().gameObject.tag = "Player" + playerNumber.ToString();

                orb.GetComponent<BallController>().Launch(launchVelocity);
                
                Debug.Log("Player " + playerNumber + " released ball with velocity: " + launchVelocity);
            }
            
        } else
        {
            if (context.phase == InputActionPhase.Started && orb != null && this != null)
            {
                if (orb.GetComponentInChildren<Collider>().Raycast(ray, out hitData, 0.5f))
                {
                    //lastOrbPosition = orb.transform.position;
                    BallGrabbed = true;
                    Debug.Log("Player " + playerNumber + " grabbed the ball.");
                }
            }
        }
    }

    void WhileBallGrabbed()
    {   
        if (screenPositionBuffer.Count == 4) screenPositionBuffer.RemoveAt(0);
        screenPositionBuffer.Add(Pointer.current.position.ReadValue());
        /*
        float sumXm = 0.0f;
        float sumYm = 0.0f;
        for (int i = 1; i < screenPositionBuffer.Count; i++)
        {
            sumXm += (screenPositionBuffer[i].x - screenPositionBuffer[i-1].x) / dpi;
            sumYm += (screenPositionBuffer[i].y - screenPositionBuffer[i-1].y) / dpi;
        }
        float avgXm = (sumXm / (float)(screenPositionBuffer.Count)) * (1.0f / Time.fixedDeltaTime) * (1.0f / INCHES_TO_METERS);
        float avgYm = (sumYm / (float)(screenPositionBuffer.Count)) * (1.0f / Time.fixedDeltaTime) * (1.0f / INCHES_TO_METERS);
        */
        orb.transform.position = mainCamera.ScreenToWorldPoint(
            new Vector3(
                screenPositionBuffer[screenPositionBuffer.Count-1].x,
                screenPositionBuffer[screenPositionBuffer.Count-1].y,
                mainCamera.nearClipPlane + BallZOffset
            )
        );
        /*
        if (debugText != null)
        {
            debugText.text = "Pointer position (pixels): " + screenPositionBuffer[screenPositionBuffer.Count-1] + "\n";
            debugText.text += "x m/s: " + avgXm + "\n";
            debugText.text += "y m/s: " + avgYm + "\n";
        }
        */
    }

    private void OnDisable() {
        if (orb != null) Destroy(orb);
    }

    public void SetPlayerNumber(int num)
    {
        playerNumber = num;
    }

    public void SetDebugTextObject(Text textObject)
    {
        debugText = textObject;
    }
}
