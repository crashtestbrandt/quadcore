using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallGrabber : MonoBehaviour
{
    public static event Action BallGrabbedByCellEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider) {
        Debug.Log("A ball entered the grabber's space.");
        collider.gameObject.transform.parent = this.transform;
        collider.gameObject.transform.position = this.transform.position;
        BallGrabbedByCellEvent();
    }
}
