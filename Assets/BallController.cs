using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    public bool autoBall = false;
    public Rigidbody rb;
    public float speed = 9.0f;
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

    public void Launch(Vector3 velocity)
    {
        rb.isKinematic = false;
        rb.velocity = velocity;
    }
}
