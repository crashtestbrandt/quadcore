using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FloorController.BallCollidedWithFloorEvent += OnReset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnReset()
    {
        Debug.Log("Reset requested!");
    }

    
}
