using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera;
    public SphereCollider ballCollider;
    bool BallGrabbed = false;

    public GameObject ball; // TODO: Remove when ball becomes rigid body
    
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (BallGrabbed) WhileBallGrabbed();
    }


    public void OnGrab(InputAction.CallbackContext context)
    {
        ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hitData;
        if (BallGrabbed)
        {
            if (context.phase == InputActionPhase.Canceled)
            {
                BallGrabbed = false;
            }
            
        } else
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (ballCollider.Raycast(ray, out hitData, 20))
                {
                    BallGrabbed = true;
                }
            }
        }
    }

    void WhileBallGrabbed()
    {
        //Vector2 MousePos = InputDevice.current.Pointer.position.ReadValue();
        Vector2 MousePos = Touchscreen.current.position.ReadValue();
        Vector3 MousePosTo3D = new Vector3(MousePos.x, MousePos.y, 0.3f);
        Vector3 WorldPosition = MainCamera.ScreenToWorldPoint(MousePosTo3D);
        Debug.Log("Ball held at " + WorldPosition);

        ball.transform.position = new Vector3(WorldPosition.x, WorldPosition.y, ball.transform.position.z);
    }


}
