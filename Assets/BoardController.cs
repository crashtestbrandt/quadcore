using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardController : MonoBehaviour
{
    public const int NumColumns = 7;
    public const int NumRows = 6;
    public float CellWidth = 0.14f;

    private List<BallGrabber> grabbers;

    private GameObject[,] board;

    public GameObject CellPrefab;
    // Start is called before the first frame update
    void Start()
    {
        board = new GameObject[6,7];

        grabbers = new List<BallGrabber>();
        for (int i = 0; i < NumColumns; i++)
        {
            grabbers.Add(Instantiate(CellPrefab, this.transform.position + (0.5f * CellWidth * this.transform.forward), Quaternion.identity, this.transform).GetComponent<BallGrabber>());
            grabbers[i].Row = 0;
            grabbers[i].Column = i;
        }
        BallGrabber.BallGrabbedByCell += OnGrabberTriggered;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrabberTriggered(int row, int column, string tag)
    {
        Debug.Log("Board was notify that cell {" + row + "," + column + "} grabbed a ball from " + tag + ".");
        if (board[row,column] == null)
        {
            board[row,column] = new GameObject();
            board[row,column].tag = tag;
        }
    }
}
