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

    public delegate void GameOverEvent(string tag);
    public static GameOverEvent GameOver;

    void Start()
    {
        if (board == null) board = new GameObject[6,7];

        grabbers = new List<BallGrabber>();
        for (int i = 0; i < NumColumns; i++)
        {
            grabbers.Add(Instantiate(CellPrefab, this.transform.position + (0.5f * CellWidth * this.transform.forward), Quaternion.identity, this.transform).GetComponent<BallGrabber>());
            grabbers[i].Row = 0;
            grabbers[i].Column = i;
        }

        // Register callbacks
        BallGrabber.BallGrabbedByCell += OnGrabberTriggered;
        GameOver += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrabberTriggered(int row, int column, string tag)
    {
        if (board == null) board = new GameObject[6,7];

        Debug.Log("Board was notified that cell {" + row + "," + column + "} grabbed a ball from " + tag + ".");
        if (board[row,column] == null)
        {
            GameObject temp = new GameObject();
            temp.tag = tag;
            board[row,column] = temp;
            if (CheckForWin(row, column, tag))
            {
              GameOver(tag);
            }
        }
    }

    public bool CheckForWin(int row, int column, string tag)
    {
        int matches = 0;

        // Check vertically
        Debug.Log("Checking for win with: " + ((board == null)? "No board!" : " a valid board."));
        for (int i = row; i < NumRows; i++)
        {
            Debug.Log("Checking (" + i + "," + column + "): " + ((board[i,column] == null) ? "null" : board[i,column].tag));
            if (board[i,column]?.tag == tag)
            {
                matches++;
                Debug.Log("Matches: " + matches);
            }
            else break;

            if (matches == 4) return true;
        }
        for (int i = row-1; i >=0; i--)
        {
            Debug.Log("Checking (" + i + "," + column + "): " + ((board[i,column] == null) ? "null" : board[i,column].tag));
            if (board[i,column]?.tag == tag)
            {
                matches++;
                Debug.Log("Matches: " + matches);
            }
            else break;

            if (matches == 4) return true;
        }

        matches = 0;

        // Check horizontally
        for (int j = column; j < NumColumns; j++)
        {
            Debug.Log("Checking (" + row + "," + j + "): " + ((board[row,j] == null) ? "null" : board[row,j].tag));
            if (board[row,j]?.tag == tag)
            {
                matches++;
                Debug.Log("Matches: " + matches);
            }
            else break;

            if (matches == 4) return true;
        }
        for (int j = column-1; j >=0; j--)
        {
            Debug.Log("Checking (" + row + "," + j + "): " + ((board[row,j] == null) ? "null" : board[row,j].tag));
            if (board[row,j]?.tag == tag)
            {
                matches++;
                Debug.Log("Matches: " + matches);
            }
            else break;

            if (matches == 4) return true;
        }

        matches = 0;

        // Check diagonally one way
        for (int i = row, j = column; i < NumRows && j < NumColumns; i++, j++)
        {
            Debug.Log("Checking (" + i + "," + j + "): " + ((board[i,j] == null) ? "null" : board[i,j].tag));
            if (board[row,j]?.tag == tag)
            {
                matches++;
                Debug.Log("Matches: " + matches);
            }
            else break;

            if (matches == 4) return true;
        }

        for (int i = row-1, j = column-1; i >= 0 && j >= 0; i--, j--)
        {
            Debug.Log("Checking (" + i + "," + j + "): " + ((board[i,j] == null) ? "null" : board[i,j].tag));
            if (board[row,j]?.tag == tag)
            {
                matches++;
                Debug.Log("Matches: " + matches);
            }
            else break;

            if (matches == 4) return true;
        }

        matches = 0;

        // Check diagonally the other way
        for (int i = row, j = column; i >= 0 && j < NumColumns; i--, j++)
        {
            Debug.Log("Checking (" + i + "," + j + ")");
            if (board[row,j]?.tag == tag)
            {
                matches++;
                Debug.Log("Matches: " + matches);
            }
            else break;

            if (matches == 4) return true;
        }

        for (int i = row+1, j = column-1; i < NumRows && j >= 0; i++, j--)
        {
            Debug.Log("Checking (" + i + "," + j + ")");
            if (board[row,j]?.tag == tag)
            {
                matches++;
                Debug.Log("Matches: " + matches);
            }
            else break;

            if (matches == 4) return true;
        }
        return false;
    }

    public void ClearBoard()
    {
        board = null;
    }

    void OnGameOver(string tag)
    {
        Debug.Log(tag + " WINS THE GAME!");
        ClearBoard();
    }
}
