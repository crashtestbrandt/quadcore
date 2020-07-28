using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Camera mainCamera;
    bool BallGrabbed = false;

    GameObject orb;
    GameObject ChosenOrbPrefab;
    public GameObject[] OrbPrefabs;

    public float BallZOffset = 0.5f;
    public float BallYOffset = 1.3f;
    public float SpeedFactor = 1.0f;

    Vector3 launchVelocity = Vector3.zero;
    Vector3 lastBallPosition = Vector3.zero;
    
    Ray ray;

    public int PlayerNumber { get; set; }

    void Awake()
    {
        Debug.Log("Created a player.");
        mainCamera = Camera.main;
    }
    void Start()
    {
        //mainCamera = Camera.main;
        //StartTurn();
        
    }

    public void StartTurn()
    {
        Debug.Log("Player starting turn");
        ChosenOrbPrefab = OrbPrefabs[PlayerNumber-1];
        orb = Instantiate(
            ChosenOrbPrefab,
            new Vector3(
                transform.position.x,
                transform.position.y + BallYOffset,
                transform.position.z + mainCamera.nearClipPlane + BallZOffset
            ),
            Quaternion.AngleAxis(45.0f, transform.right)
        );
        /*
        ball = Instantiate(
            BallPrefab,
            new Vector3(
                transform.position.x,
                transform.position.y + BallYOffset,
                transform.position.z + mainCamera.nearClipPlane + BallZOffset
            ),
            Quaternion.AngleAxis(45.0f, transform.right)
            );
        */
        //if (PlayerNumber == 1) ball.GetComponentInChildren<Renderer>().material.color = Color.red;
        //else if (PlayerNumber == 2) ball.GetComponentInChildren<Renderer>().material.color = Color.blue;
        lastBallPosition = orb.transform.position;
        launchVelocity = Vector3.zero;
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

                //orb.tag = "Player" + PlayerNumber.ToString();
                orb.GetComponentInChildren<Collider>().gameObject.tag = "Player" + PlayerNumber.ToString();

                //orb.GetComponentInChildren<BallController>().Launch(SpeedFactor * launchVelocity);
                orb.GetComponent<BallController>().Launch(SpeedFactor * launchVelocity);
                
                Debug.Log("Player " + PlayerNumber + " released ball with velocity: " + launchVelocity);
            }
            
        } else
        {
            if (context.phase == InputActionPhase.Started && orb != null && this != null)
            {
                if (orb.GetComponentInChildren<Collider>().Raycast(ray, out hitData, 0.5f))
                {
                    lastBallPosition = orb.transform.position;
                    BallGrabbed = true;
                    Debug.Log("Player " + PlayerNumber + " grabbed the ball.");
                }
            }
        }
    }

    void WhileBallGrabbed()
    {
        Vector2 pointerPosition = Pointer.current.position.ReadValue();
        Vector3 pointerPositionToWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(pointerPosition.x, pointerPosition.y, mainCamera.nearClipPlane + BallZOffset)
        );

        orb.transform.position = pointerPositionToWorldPosition;

        launchVelocity = ((orb.transform.position.y - lastBallPosition.y) * orb.transform.up +
            (orb.transform.position.x - lastBallPosition.x) * orb.transform.right) / Time.fixedDeltaTime;

        lastBallPosition = orb.transform.position;
    }

    private void OnDisable() {
        if (orb != null) Destroy(orb);
    }
}
