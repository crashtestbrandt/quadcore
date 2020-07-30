using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System;

public class BallController : MonoBehaviour
{
    public delegate void ThrowTimerDelegate();
    public bool autoBall = false;
    public Rigidbody rb;
    public float speed = 9.0f;
    public float ThrowTimerSeconds = 7.0f;

    public static ThrowTimerDelegate ThrowTimer;
    // Start is called before the first frame update
    void Start()
    {
        rb.isKinematic = true;
        if (autoBall) Launch(speed * transform.up);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void Launch(Vector3 velocity)
    {
        Debug.Log("Orb launching with velocity: " + velocity);
        rb.isKinematic = false;
        rb.velocity = velocity;
        await Task.Delay(TimeSpan.FromSeconds(ThrowTimerSeconds));
        if (this != null)
        {
            ThrowTimer();
            //GameObject.Destroy(this);
        }
    }
}
