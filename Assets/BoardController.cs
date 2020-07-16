using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public int NumColumns = 7;
    public int NumRows = 6;
    public float CellWidth = 0.14f;

    private List<BallGrabber> grabbers;

    public GameObject CellPrefab;
    // Start is called before the first frame update
    void Start()
    {
        grabbers = new List<BallGrabber>();
        for (int i = 0; i < NumColumns; i++)
        {
            grabbers.Add(Instantiate(CellPrefab, this.transform.position + (0.5f * CellWidth * this.transform.forward), Quaternion.identity, this.transform).GetComponent<BallGrabber>());
            grabbers[i].Row = 0;
            grabbers[i].Column = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
