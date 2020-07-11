using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    
    private Touch touch;
    private float speedModifier;
    public GameObject PaperBall;
    private bool alreadyTouched;
    
    // Start is called before the first frame update
    void Start()
    {
        speedModifier = 0.001f;
        alreadyTouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved) {
                transform.position = new Vector3(
                    transform.position.x + touch.deltaPosition.x * speedModifier,
                    transform.position.y,
                    transform.position.z + touch.deltaPosition.y * speedModifier
                );
                alreadyTouched = true;
                PaperBall.GetComponent<SpringJoint>().spring = 25;
            }
        } else {
            if (alreadyTouched) {
                // Destroy(spring);
                PaperBall.GetComponent<SpringJoint>().spring = 0;
            }
        }
    }
}

