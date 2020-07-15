using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Camera mainCamera;
    bool BallGrabbed = false;

    GameObject ball;
    public GameObject BallPrefab;

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

        ball = Instantiate(
            BallPrefab,
            new Vector3(
                transform.position.x,
                transform.position.y + BallYOffset,
                transform.position.z + mainCamera.nearClipPlane + BallZOffset
            ),
            Quaternion.AngleAxis(45.0f, transform.right)
            );
        if (PlayerNumber == 1) ball.GetComponentInChildren<Renderer>().material.color = Color.red;
        else if (PlayerNumber == 2) ball.GetComponentInChildren<Renderer>().material.color = Color.blue;
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
                ball.GetComponentInChildren<BallController>().Launch(SpeedFactor * launchVelocity);
            }
            
        } else
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (ball.GetComponentInChildren<Collider>().Raycast(ray, out hitData, 20))
                {
                    lastBallPosition = ball.transform.position;
                    BallGrabbed = true;
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

        ball.transform.position = pointerPositionToWorldPosition;

        launchVelocity = ((ball.transform.position.y - lastBallPosition.y) * ball.transform.up +
            (ball.transform.position.x - lastBallPosition.x) * ball.transform.right) / Time.fixedDeltaTime;

        lastBallPosition = ball.transform.position;
    }

    private void OnDisable() {
        if (ball != null) Destroy(ball);
    }
}
