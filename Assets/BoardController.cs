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
        Debug.Log("Board was notified that cell {" + row + "," + column + "} grabbed a ball from " + tag + ".");
        if (board[row,column] == null)
        {
            board[row,column] = new GameObject();
            board[row,column].tag = tag;
            if (CheckForWin(row, column, tag))
            {
                Debug.Log(tag + " WINS THE GAME!");
            }
        }
    }

    bool CheckForWin(int row, int column, string tag)
    {
        int matches = 0;

        // Check vertically
        Debug.Log("Checking for win with: " + ((board == null)? "No board!" : " a valid board."));
        for (int i = row; i < NumRows; i++)
        {
            Debug.Log("Checking (" + i + "," + column + ")");
            if (board[i,column]?.tag == tag) matches++;
            else break;

            if (matches == 4) return true;
        }
        for (int i = row-1; i > NumRows-5; i--)
        {
            if (board[i,column]?.tag == tag) matches++;
            else break;

            if (matches == 4) return true;
        }

        matches = 0;

        // Check horizontally
        for (int j = column; j < NumColumns; j++)
        {
            if (board[row,j]?.tag == tag) matches++;
            else break;

            if (matches == 4) return true;
        }
        for (int j = column-1; j > NumColumns-5; j--)
        {
            if (board[row,j]?.tag == tag) matches++;
            else break;

            if (matches == 4) return true;
        }

        matches = 0;

        // Check diagonally one way
        for (int i = row, j = column; i < NumRows && j < NumColumns; i++, j++)
        {
            if (board[row,j]?.tag == tag) matches++;
            else break;

            if (matches == 4) return true;
        }

        for (int i = row-1, j = column-1; i > NumRows-5 && j < NumColumns-5; i--, j--)
        {
            if (board[row,j]?.tag == tag) matches++;
            else break;

            if (matches == 4) return true;
        }

        matches = 0;

        // Check diagonally the other way
        for (int i = row, j = column; i > NumRows-5 && j < NumColumns; i--, j++)
        {
            if (board[row,j]?.tag == tag) matches++;
            else break;

            if (matches == 4) return true;
        }

        for (int i = row+1, j = column-1; i > NumRows && j < NumColumns-5; i++, j--)
        {
            if (board[row,j]?.tag == tag) matches++;
            else break;

            if (matches == 4) return true;
        }
        return false;
    }
}
