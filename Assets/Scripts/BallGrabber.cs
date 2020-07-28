using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallGrabber : MonoBehaviour
{
    //public delegate void BallGrabbedByCellEvent(int row, int column, string tag);
    public delegate void BallGrabbedByCellEvent(int row, int column, GameObject ball);

    public static BallGrabbedByCellEvent BallGrabbedByCell;
    public float CellWidth = 0.14f;
    public float NumColums = 7;
    public float NumRows = 6;
    private int column;
    public int Column
    {
        get { return column; }
        set
        {
            column = value;
            //this.transform.position = this.transform.position + (column * CellWidth - 0.5f * NumColums * CellWidth + 0.5f * CellWidth) * this.transform.right;
            this.transform.position =
            this.transform.position + (CellWidth * (column - 0.5f * NumColums + 0.5f)) * this.transform.right;
        }
    }

    private int row;
    public int Row { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = this.transform.position + (0.5f * CellWidth) * this.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider) {
        Debug.Log("A ball entered the grabber's space.");

        collider.gameObject.transform.parent = null;
        collider.gameObject.transform.position = this.transform.position;

        BallGrabbedByCell(Row, Column, collider.gameObject);

        if (this.Row < NumRows - 1)
        {
            this.Row = this.Row + 1;
            this.transform.position = this.transform.position + CellWidth * this.transform.up;
        }
        else
        {
            this.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
