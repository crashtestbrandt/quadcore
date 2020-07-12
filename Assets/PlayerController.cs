using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera;
    bool BallGrabbed = false;

    public GameObject ball;

    public float BallZOffset = 0.5f;
    public float SpeedFactor = 1.0f;

    Vector3 launchVelocity = Vector3.zero;
    Vector3 lastBallPosition = Vector3.zero;
    
    Ray ray;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (BallGrabbed) WhileBallGrabbed();
    }


    public void OnGrab(InputAction.CallbackContext context)
    {
        ray = MainCamera.ScreenPointToRay(Pointer.current.position.ReadValue());
        RaycastHit hitData;
        if (BallGrabbed)
        {
            if (context.phase == InputActionPhase.Canceled)
            {
                BallGrabbed = false;
                Debug.Log("Ball launced with velocity:\t" + launchVelocity);
                Debug.Log("\tWith speed factor:\t\t" + SpeedFactor * launchVelocity);
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
        Vector3 pointerPositionToWorldPosition = MainCamera.ScreenToWorldPoint(
            new Vector3(pointerPosition.x, pointerPosition.y, MainCamera.nearClipPlane + BallZOffset)
        );
        Debug.Log("\nPointer Position:\t" + pointerPosition);
        Debug.Log("Pointer Position to World:\t" + pointerPositionToWorldPosition);

        ball.transform.position = pointerPositionToWorldPosition;

        launchVelocity = ((ball.transform.position.y - lastBallPosition.y) * ball.transform.up +
            (ball.transform.position.x - lastBallPosition.x) * ball.transform.right) / Time.fixedDeltaTime;

        lastBallPosition = ball.transform.position;
    }
}
