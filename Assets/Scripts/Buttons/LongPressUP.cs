using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongPressUP : MonoBehaviour
{
    private GameObject objectToMove;
    private bool isClicked;
    private void OnMouseDown()
    {
        isClicked = true;
    }
    private void OnMouseUp()
    {
        isClicked = false;
    }

    private void Update()
    {
        if(isClicked)
        {
            objectToMove = Selection.GlobalGameObject;
            if (objectToMove != null )
            {
                objectToMove.transform.position += new Vector3(0, 0.01f, 0);
            }
        }  
    }
}
