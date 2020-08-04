using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity;

public class RemotePlayerController : MonoBehaviour, IPlayerController
{
    Text debugText = null;
    int playerNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDebugTextObject(Text textObject)
    {
        debugText = textObject;
    }

    public void SetPlayerNumber(int num)
    {
        playerNumber = num;
    }

    public void StartTurn()
    {

    }
}
