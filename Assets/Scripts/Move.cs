using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public GameObject objectToMove;
    // Start is called before the first frame update
    public void MoveUp()
    {
        objectToMove.transform.position += new Vector3(0, 5, 0);
    }
    public void MoveDown()
    {
        objectToMove.transform.position += new Vector3(0, -5, 0);
    }
    public void MoveLeft()
    {
        objectToMove.transform.position += new Vector3(-5, 0, 0);
    }
    public void MoveRight()
    {
        objectToMove.transform.position += new Vector3(5, 0, 0);
    }
    public void MoveUpGradually()
    {
        objectToMove.transform.position += new Vector3(0, 1, 0);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
