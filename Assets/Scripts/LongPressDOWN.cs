using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongPressDOWN : MonoBehaviour
{
    public GameObject objectToMove;
    private bool isClicked;
    private void OnMouseDown()
    {
        isClicked = true;
        Debug.Log("Click");
    }
    private void OnMouseUp()
    {
        isClicked = false;
    }

    private void Update()
    {
        if(isClicked)
        {
            objectToMove.transform.position += new Vector3(0, -0.01f, 0);
        }
        
    }
}
