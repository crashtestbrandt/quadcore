using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IPlayerController
{
    Camera mainCamera;
    bool BallGrabbed = false;

    GameObject orb;
    GameObject ChosenOrbPrefab;
    public GameObject[] OrbPrefabs;

    public Text DebugText { get; set; } = null;

    public float BallZOffset = 0.5f;
    public float BallYOffset = 1.3f;
    public float SpeedFactor = 1.0f;

    

    // TODO: Make these into an array
    Vector3 lastOrbPosition = Vector3.zero;
    Vector3 secondToLastOrbPosition = Vector3.zero;
    Vector3 thirdToLastOrbPosition = Vector3.zero;
    Vector3 fourthToLastOrbPosition = Vector3.zero;
    
    Ray ray;

    int playerNumber;

    void Awake()
    {
        Debug.Log("Created a player.");
        mainCamera = Camera.main;
    }

    public void StartTurn()
    {
        Debug.Log("Player starting turn");
        ChosenOrbPrefab = OrbPrefabs[playerNumber-1];
        orb = Instantiate(
            ChosenOrbPrefab,
            new Vector3(
                transform.position.x,
                transform.position.y + BallYOffset,
                transform.position.z + mainCamera.nearClipPlane + BallZOffset
            ),
            Quaternion.AngleAxis(45.0f, transform.right)
        );

        fourthToLastOrbPosition = orb.transform.position;
        thirdToLastOrbPosition = orb.transform.position;
        secondToLastOrbPosition = orb.transform.position;
        lastOrbPosition = orb.transform.position;
        
        if (DebugText != null)
        {
            DebugText.text = "Current position:\t\t\t\t" + orb.transform.position;
            DebugText.text += "\nLast position:\t\t\t\t\t" + lastOrbPosition;
            DebugText.text += "\nSecond-to-last position:\t" + secondToLastOrbPosition;
            DebugText.text += "\nThird-to-last position:\t\t" + thirdToLastOrbPosition;
            DebugText.text += "\nFourth-to-last position:\t" + fourthToLastOrbPosition;
        }
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

                Vector3[] velocities = {
                    ((orb.transform.position.y - lastOrbPosition.y) * orb.transform.up +
                        (orb.transform.position.x - lastOrbPosition.x) * orb.transform.right) / Time.fixedDeltaTime,
                    ((orb.transform.position.y - secondToLastOrbPosition.y) * orb.transform.up +
                        (orb.transform.position.x - secondToLastOrbPosition.x) * orb.transform.right) / (2*Time.fixedDeltaTime),
                    ((orb.transform.position.y - thirdToLastOrbPosition.y) * orb.transform.up +
                        (orb.transform.position.x - thirdToLastOrbPosition.x) * orb.transform.right) / (3*Time.fixedDeltaTime),
                    ((orb.transform.position.y - fourthToLastOrbPosition.y) * orb.transform.up +
                        (orb.transform.position.x - fourthToLastOrbPosition.x) * orb.transform.right) / (4*Time.fixedDeltaTime),
                };
                Vector3 launchVelocity = Vector3.zero;
                foreach (Vector3 vel in velocities)
                {
                    if (vel.magnitude > launchVelocity.magnitude)
                    {
                        launchVelocity = vel;
                    }
                }

                if (DebugText != null)
                {
                    DebugText.text = "Current position:\t\t\t\t" + orb.transform.position;
                    DebugText.text += "\nLast position:\t\t\t\t\t" + lastOrbPosition;
                    DebugText.text += "\nSecond-to-last position:\t" + secondToLastOrbPosition;
                    DebugText.text += "\nThird-to-last position:\t\t" + thirdToLastOrbPosition;
                    DebugText.text += "\nFourth-to-last position:\t" + fourthToLastOrbPosition;
                    DebugText.text += "\n\nBall launching with velocity: " + launchVelocity;
                }
                
                orb.GetComponentInChildren<Collider>().gameObject.tag = "Player" + playerNumber.ToString();

                orb.GetComponent<BallController>().Launch(SpeedFactor * launchVelocity);
                
                Debug.Log("Player " + playerNumber + " released ball with velocity: " + launchVelocity);
            }
            
        } else
        {
            if (context.phase == InputActionPhase.Started && orb != null && this != null)
            {
                if (orb.GetComponentInChildren<Collider>().Raycast(ray, out hitData, 0.5f))
                {
                    lastOrbPosition = orb.transform.position;
                    BallGrabbed = true;
                    Debug.Log("Player " + playerNumber + " grabbed the ball.");
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

        orb.transform.position = pointerPositionToWorldPosition;
        /*
        launchVelocity = ((orb.transform.position.y - lastOrbPosition.y) * orb.transform.up +
            (orb.transform.position.x - lastOrbPosition.x) * orb.transform.right) / Time.fixedDeltaTime;
        */

        if (DebugText != null)
        {
            DebugText.text = "Current position:\t\t\t\t" + orb.transform.position;
            DebugText.text += "\nLast position:\t\t\t\t\t" + lastOrbPosition;
            DebugText.text += "\nSecond-to-last position:\t" + secondToLastOrbPosition;
            DebugText.text += "\nThird-to-last position:\t\t" + thirdToLastOrbPosition;
            DebugText.text += "\nFourth-to-last position:\t" + fourthToLastOrbPosition;
        }

        fourthToLastOrbPosition = thirdToLastOrbPosition;
        thirdToLastOrbPosition = secondToLastOrbPosition;
        secondToLastOrbPosition = lastOrbPosition;
        lastOrbPosition = orb.transform.position;

        
    }

    private void OnDisable() {
        if (orb != null) Destroy(orb);
    }

    public void SetPlayerNumber(int num)
    {
        playerNumber = num;
    }
}

public interface IPlayerController
{
    void SetPlayerNumber(int num);
    void StartTurn();
}
