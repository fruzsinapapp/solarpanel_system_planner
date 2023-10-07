using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro; // Import the TMPro namespace if it's not already imported
public class GetPositionsButton : MonoBehaviour
{
    public TextMeshPro myTextMeshPro;
    ProjectionCode pc = new ProjectionCode();
    public void GetPositions()
    {
        foreach(var dot in PenTool.listOfDots)
        {
            PrintPositions(dot);
            pc.ProjectionCodeImpl(dot.transform.position);
        }
    }

    private void PrintPositions(GameObject dot)
    {
        myTextMeshPro.text = "1. x: " + dot.transform.position.x + ", y: " + dot.transform.position.y + ", z: " + dot.transform.position.z;
        Debug.Log("1. x: " + dot.transform.position.x + ", y: " + dot.transform.position.y + ", z: " + dot.transform.position.z);
    }

    public List<Vector2> GetPositionsForPython()
    {
        List<Vector2> listOfProjectedDots = new List<Vector2>();
        foreach (var dot in PenTool.listOfDots)
        {
            listOfProjectedDots.Add(pc.ProjectionForPython(dot.transform.position));
        }
        return listOfProjectedDots;
    }
}
