using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IPlayerController
{
    Camera mainCamera;
    bool BallGrabbed = false;

    GameObject orb;
    GameObject ChosenOrbPrefab;
    public GameObject[] OrbPrefabs;

    Text debugText = null;

    public float BallZOffset = 0.5f;
    public float BallYOffset = 1.3f;
    public float YSpeedFactor = 1.0f;
    public float XSpeedFactor = 0.5f;
    public float MaxSpeed = 30.0f;


    Vector3 screenDiagonal;
    Vector3 screenDiagonalMaxInWorld;
    Vector3 screenDiagonalMinInWorld;
   // Vector2 pixelsPerUnit;
   float pixelsPerUnit;
   float screenPortionPerFrame = 0.0f;

    
    
    Ray ray;

    int playerNumber;

    List<Vector2> screenPositionBuffer;

    void Awake()
    {
        screenPositionBuffer = new List<Vector2>();
        Debug.Log("Created a player.");
        mainCamera = Camera.main;
        screenDiagonal = new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane + BallZOffset);
        screenDiagonalMaxInWorld = mainCamera.ScreenToWorldPoint(screenDiagonal);
        screenDiagonalMinInWorld = mainCamera.ScreenToWorldPoint(
            new Vector3(0.0f, 0.0f, mainCamera.nearClipPlane + BallZOffset)
        );
        /*
        pixelsPerUnit = new Vector2(
            screenDiagonal.x / (screenDiagonalMaxInWorld.x - screenDiagonalMinInWorld.x),
            screenDiagonal.y  / (screenDiagonalMaxInWorld.y - screenDiagonalMinInWorld.y)
            );
        */
        pixelsPerUnit = screenDiagonal.x / (screenDiagonalMaxInWorld.x - screenDiagonalMinInWorld.x);
    }

    public void StartTurn()
    {
        if (debugText != null)
        {
            debugText.text = "Screen diagonal (pixels): " + screenDiagonal + "\n";
            debugText.text += "Screen diagonal (world, max): " + screenDiagonalMaxInWorld + "\n";
            debugText.text += "Screen diagonal (world, min): " + screenDiagonalMinInWorld + "\n";
            debugText.text += "Pixels per unit: " + pixelsPerUnit + "\n";

        }
        Debug.Log("Player starting turn");
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

                Vector2 sum = Vector2.zero;
                foreach (Vector2 vel in screenPositionBuffer)
                {
                    sum += vel;
                }
                Vector2 launch2D = sum / screenPositionBuffer.Count;
                Vector3 launchDirection = new Vector3(launch2D.x - 0.5f * screenDiagonal.x, 0.71f * launch2D.y,  0.71f * launch2D.y).normalized;
                Vector3 launchVelocity = screenPortionPerFrame * MaxSpeed * launchDirection;
                launchVelocity.x = launchVelocity.x * XSpeedFactor;
                launchVelocity.y = launchVelocity.y * YSpeedFactor;
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
        float sum = 0.0f;
        for (int i = 1; i < screenPositionBuffer.Count; i++)
        {
            sum += ((screenPositionBuffer[i] - screenPositionBuffer[i-1])).magnitude;
        }
        float avg = sum / (float)(screenPositionBuffer.Count - 1);
        screenPortionPerFrame = avg / screenDiagonal.magnitude;


        orb.transform.position = mainCamera.ScreenToWorldPoint(
            new Vector3(
                screenPositionBuffer[screenPositionBuffer.Count-1].x,
                screenPositionBuffer[screenPositionBuffer.Count-1].y,
                mainCamera.nearClipPlane + BallZOffset
            )
        );
        
        if (debugText != null)
        {
            debugText.text = "Screen diagonal (pixels): " + screenDiagonal + "\n";
            debugText.text += "Screen diagonal (world, max): " + screenDiagonalMaxInWorld + "\n";
            debugText.text += "Screen diagonal (world, min): " + screenDiagonalMinInWorld + "\n";
            debugText.text += "Pixels per unit: " + pixelsPerUnit + "\n";
            debugText.text += "Pointer position (pixels): " + screenPositionBuffer[screenPositionBuffer.Count-1] + "\n";
            debugText.text += "Orb position (world):  " + orb.transform.position + "\n";
            debugText.text += "Avg. portion of screen per frame: " + screenPortionPerFrame + "\n";
        }   
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

public interface IPlayerController
{
    void SetDebugTextObject(Text textObject);
    void SetPlayerNumber(int num);
    void StartTurn();
}
