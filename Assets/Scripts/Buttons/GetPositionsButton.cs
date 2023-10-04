using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPositionsButton : MonoBehaviour
{
    public void GetPositions()
    {
        foreach(var dot in PenTool.listOfDots)
        {
            Debug.Log("1. x: " + dot.transform.position.x + ", y: " + dot.transform.position.y + ", z: " + dot.transform.position.z);
        }
    }
}
