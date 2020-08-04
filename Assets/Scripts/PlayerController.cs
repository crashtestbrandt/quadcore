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

    Vector2 lastDotPos;
    float lastDotTime;
    List<Vector2> dotVelocityBuffer;

    void Awake()
    {
        dpi = Screen.dpi;
        lastDotPos = Vector2.zero;
        lastDotTime = Time.time;
        Debug.Log("Screen DPI: " + dpi);
        dotVelocityBuffer = new List<Vector2>();
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

                float sumXmps = 0.0f;
                float sumYmps = 0.0f;
                for (int i = 1; i < dotVelocityBuffer.Count; i++)
                {
                    sumXmps += dotVelocityBuffer[i].x / (dpi * INCHES_TO_METERS);
                    sumYmps += dotVelocityBuffer[i].y / (dpi * INCHES_TO_METERS);
                }
                float avgXmps = (sumXmps / (float)(dotVelocityBuffer.Count));
                float avgYmps = (sumYmps / (float)(dotVelocityBuffer.Count));
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
                    lastDotPos = Pointer.current.position.ReadValue();
                    lastDotTime = Time.time;
                }
            }
        }
    }

    void WhileBallGrabbed()
    {   
        Vector2 thisDotPos = Pointer.current.position.ReadValue();
        float thisDotTime = Time.time;
        float dt = thisDotTime - lastDotTime;
        if (dotVelocityBuffer.Count == 4) dotVelocityBuffer.RemoveAt(0);
        dotVelocityBuffer.Add(
            new Vector2(
                (thisDotPos.x - lastDotPos.x) / dt, (thisDotPos.y - lastDotPos.y) / dt
                )
            );
        orb.transform.position = mainCamera.ScreenToWorldPoint(
            new Vector3(
                thisDotPos.x,
                thisDotPos.y,
                mainCamera.nearClipPlane + BallZOffset
            )
        );

        lastDotPos = thisDotPos;
        lastDotTime = thisDotTime;

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
