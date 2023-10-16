using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro; // Import the TMPro namespace if it's not already imported
public class GetPositionsButton : MonoBehaviour
{
    ProjectionCode pc = new ProjectionCode();

    #region This is for testing, when pushing the "POSITIONS" button
    public TextMeshPro myTextMeshPro;
    public void GetPositions()
    {
        Transform dot1 = PenTool.listOfDotsWithText[0].transform.Find("Dot");
        Transform dot2 = PenTool.listOfDotsWithText[1].transform.Find("Dot");
        Vector3 vectorBetweenDots = dot1.transform.position - dot2.transform.position;

        //Vector3 vectorBetweenDots = PenTool.listOfDots[0].transform.position - PenTool.listOfDots[1].transform.position;
        Vector3 projectionDirection = new Vector3(0, 0, 1);
        float angle = Vector3.Angle(vectorBetweenDots, projectionDirection);
        Debug.Log("Angle: " + angle);

        
        foreach(var dotWithText in PenTool.listOfDotsWithText)
        {
            Transform dot = dotWithText.transform.Find("Dot");
            PrintPositions(dotWithText);

            pc.ProjectionCodeImpl(dot.transform.position);
        }
        
    }

    private void PrintPositions(GameObject dotWithText)
    {
        Transform dot = dotWithText.transform.Find("Dot");
        myTextMeshPro.text = "1. x: " + dot.transform.position.x + ", y: " + dot.transform.position.y + ", z: " + dot.transform.position.z;
        Debug.Log("1. x: " + dot.transform.position.x + ", y: " + dot.transform.position.y + ", z: " + dot.transform.position.z);
    }
    #endregion

    public List<Vector2> GetPositionsForPython()
    {
        List<Vector2> listOfProjectedDots = new List<Vector2>();
        foreach (var dotWithText in PenTool.listOfDotsWithText)
        {
            Transform dot = dotWithText.transform.Find("Dot");
            Vector3 newCoordinates = PenTool.zeroMarkerScript.UseRealCoordinates(dot);
            listOfProjectedDots.Add(pc.ProjectionForPython(newCoordinates));
        }
        return listOfProjectedDots;
    }
}
