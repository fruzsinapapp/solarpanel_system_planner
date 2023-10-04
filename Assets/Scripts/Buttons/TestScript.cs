using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject objectToMove;
    private bool isTouched = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouched)
        {
            //objectToMove = objectToMove;
            if (objectToMove != null)
            {
                objectToMove.transform.position += new Vector3(0, 0.01f, 0);
            }
        }

    }
    public void TouchStarted()
    {
        isTouched= true;
        Debug.Log("TOUCHED");
    }
    public void TouchEnded()
    {
        isTouched = false;
    }
}
