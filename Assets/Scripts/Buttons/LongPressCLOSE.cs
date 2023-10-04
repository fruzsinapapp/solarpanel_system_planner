using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongPressCLOSE : MonoBehaviour
{
    public GameObject objectToMove;
    private bool isTouched = false;

    private void Update()
    {
        if (isTouched)
        {
            objectToMove = Selection.GlobalGameObject;
            if (objectToMove != null)
            {
                objectToMove.transform.position += new Vector3(0, 0, -0.01f);
            }
        }
    }
    public void TouchStarted()
    {
        isTouched = true;
    }
    public void TouchEnded()
    {
        isTouched = false;
    }
}
