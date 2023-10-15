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
        GameObject dot1 = PenTool.listOfDotsWithText[0].gameObject.GetComponent<GameObject>();
        GameObject dot2 = PenTool.listOfDotsWithText[1].gameObject.GetComponent<GameObject>();
        Vector3 vectorBetweenDots = dot1.transform.position - dot2.transform.position;

        //Vector3 vectorBetweenDots = PenTool.listOfDots[0].transform.position - PenTool.listOfDots[1].transform.position;
        Vector3 projectionDirection = new Vector3(0, 0, 1);
        float angle = Vector3.Angle(vectorBetweenDots, projectionDirection);
        Debug.Log("Angle: " + angle);

        /*
        foreach(var dot in PenTool.listOfDots)
        {
            PrintPositions(dot);
            pc.ProjectionCodeImpl(dot.transform.position);
        }
        */
    }

    private void PrintPositions(GameObject dot)
    {
        myTextMeshPro.text = "1. x: " + dot.transform.position.x + ", y: " + dot.transform.position.y + ", z: " + dot.transform.position.z;
        Debug.Log("1. x: " + dot.transform.position.x + ", y: " + dot.transform.position.y + ", z: " + dot.transform.position.z);
    }
    #endregion

    public List<Vector2> GetPositionsForPython()
    {
        List<Vector2> listOfProjectedDots = new List<Vector2>();
        foreach (var dotWithText in PenTool.listOfDotsWithText)
        {
            GameObject dot = dotWithText.gameObject.GetComponent<GameObject>();
            listOfProjectedDots.Add(pc.ProjectionForPython(dot.transform.position));
        }
        return listOfProjectedDots;
    }
}
