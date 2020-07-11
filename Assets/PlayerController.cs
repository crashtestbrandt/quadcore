using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera;

    bool BallGrabbed = false;
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
        if (context.phase == InputActionPhase.Started)
        {
            BallGrabbed = true;
            Debug.Log("Ball grabbed at " + Mouse.current.position.ReadValue());
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            BallGrabbed = false;
            Debug.Log("Ball released at " + Mouse.current.position.ReadValue());
        }
        //Debug.Log("Ball grabbed");
        //Debug.Log(context);
        //Debug.Log(Mouse.current.position.ReadValue());
    }

    void WhileBallGrabbed()
    {
        Debug.Log("Ball still held at " + Mouse.current.position.ReadValue());

    }


}
