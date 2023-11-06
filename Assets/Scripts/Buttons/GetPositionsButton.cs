using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro; // Import the TMPro namespace if it's not already imported
using Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes;
using System;

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

    public List<Vector2> NormalizeCoordinates(List<Vector2> coordinatesToNormalize)
    {
        // Find the minimum x and y values
        float minX = 0;
        float minY = 0;

        foreach (Vector2 coordinate in coordinatesToNormalize)
        {
            if (coordinate.x < minX)
                minX = coordinate.x;
            if (coordinate.y < minY)
                minY = coordinate.y;
        }

        // Calculate the shift values
        float shiftX = Math.Abs(minX);
        float shiftY = Math.Abs(minY);

        // Create an array for normalized coordinates
        List<Vector2> normalizedCoordinates = new List<Vector2>();

        // Add the shift values to the original coordinates
        foreach (Vector2 coordinate in coordinatesToNormalize)
        {
            float x = coordinate.x + shiftX;
            float y = coordinate.y + shiftY;
            Vector2 blabla = new Vector2(x, y);
            normalizedCoordinates.Add(blabla);
            Debug.Log("[NEW] Normalized position: " + blabla);
        }

        return normalizedCoordinates;
    }

    public List<Vector2> DefineCoordinatesToSendToAzure()
    {
        List<Vector2> listOfProjectedDots = new List<Vector2>();
        foreach (var dotWithText in PenTool.listOfDotsWithText)
        {
            Transform dot = dotWithText.transform.Find("Dot");
            Debug.Log("[NEW] Original position: " + dot.transform.position);
        }

        List<Vector3> listOfVectors = new List<Vector3>();
        //get dots
        foreach (var dotWithText in PenTool.listOfDotsWithText)
        {
            Transform dot = dotWithText.transform.Find("Dot");
            //listOfVectors.Add(dot.transform.position);
            listOfProjectedDots.Add(pc.ProjectionForPython(dot.transform.position, 0));
            Debug.Log("[NEW] projected position: " + pc.ProjectionForPython(dot.transform.position, 0));
            /*
            Vector3 newCoordinates = PenTool.zeroMarkerScript.UseRealCoordinates(dot);
            Debug.Log("[NEW] origo position: " + newCoordinates);
            
            */
        }
        List<Vector2> listToReturn = NormalizeCoordinates(listOfProjectedDots);
        /*
        foreach (var dotWithText in PenTool.listOfDotsWithText)
        {
            Transform dot = dotWithText.transform.Find("Dot");
            float originalZ = dot.transform.position.z;
            Vector3 newCoordinates = PenTool.zeroMarkerScript.UseRealCoordinates(dot);
            Debug.Log("[NEW] origo position: " + newCoordinates);
            listOfProjectedDots.Add(pc.ProjectionForPython(newCoordinates, originalZ));
        }
        */
        return listToReturn;
    }
}
