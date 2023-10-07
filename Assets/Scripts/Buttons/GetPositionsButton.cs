using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro; // Import the TMPro namespace if it's not already imported
public class GetPositionsButton : MonoBehaviour
{
    public TextMeshPro myTextMeshPro;
public void GetPositions()
    {
        ProjectionCode pc = new ProjectionCode();
        //myTextMeshPro.text = PenTool.listOfDots[0].transform.position.x.ToString();
        foreach(var dot in PenTool.listOfDots)
        {
            myTextMeshPro.text = "1. x: " + dot.transform.position.x + ", y: " + dot.transform.position.y + ", z: " + dot.transform.position.z;
            Debug.Log("1. x: " + dot.transform.position.x + ", y: " + dot.transform.position.y + ", z: " + dot.transform.position.z);
            pc.ProjectionCodeImpl(dot.transform.position);
        }
    }
}
