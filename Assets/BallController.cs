using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 9.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = speed * new Vector3(0.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
