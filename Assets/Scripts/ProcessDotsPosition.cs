using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessDotsPosition : MonoBehaviour
{
    private float shiftX = 0f;
    private float shiftY = 0f;
    private float shiftZ = 0f;

    void Start()
    {

    }
    public void CallProcessDotsPosition()
    {
        GameObject parentObject = new GameObject();
        parentObject.name = "ParentObject";

        List<Vector3> listOfDotsCoordinates = GetListOfDotsCoordinates();
        Vector3 center = CalculateCenterOfGameObjects(listOfDotsCoordinates);
        UnityEngine.Debug.Log("[CENTER] Original center: " + center);

        parentObject.transform.position = center;
        DebugGameObjectCoordinates(listOfDotsCoordinates, "BEFORE: ");
        MakeDotsChildren(parentObject);
        DebugGameObjectCoordinates(listOfDotsCoordinates, "AFTER: ");
        DoRotation(parentObject, center);
        DebugGameObjectCoordinates(listOfDotsCoordinates, "AFTER ROTATION: ");
        List<Vector3> normalizedCoordintes = NormalizeCoordinates(listOfDotsCoordinates);
        DebugGameObjectCoordinates(normalizedCoordintes, "NORMALIZED AFTER ROTATION: ");
    }
    public List<Vector3> NormalizeCoordinates(List<Vector3> coordinatesToNormalize)
    {
        float minX = 100;
        float minY = 100;
        float minZ = 100;

        foreach (Vector3 coordinate in coordinatesToNormalize)
        {
            if (coordinate.x < minX)
                minX = coordinate.x;
            if (coordinate.y < minY)
                minY = coordinate.y;
            if (coordinate.z < minZ)
                minZ = coordinate.z;
        }

        // Calculate the shift values
        shiftX = Math.Abs(minX);
        shiftY = Math.Abs(minY);
        shiftZ = Math.Abs(minZ);

        // Create an array for normalized coordinates
        List<Vector3> normalizedCoordinates = new List<Vector3>();

        // Add the shift values to the original coordinates
        foreach (Vector3 coordinate in coordinatesToNormalize)
        {
            float x = coordinate.x + shiftX;
            float y = coordinate.y + shiftY;
            float z = coordinate.z + shiftZ;
            Vector3 normalized = new Vector3(x, y, z);
            normalizedCoordinates.Add(normalized);
        }

        return normalizedCoordinates;
    }
    public void DoRotation(GameObject parentObject, Vector3 center)
    {
        float targetRotationAngle = 45.0f; //CUSTOM
        parentObject.transform.RotateAround(center, Vector3.left, targetRotationAngle);      
    }
    public void MakeDotsChildren(GameObject parentObject)
    {
        foreach (var dotWithText in PenTool.listOfDotsWithText)
        {
            Transform dot = dotWithText.transform.Find("Dot");
            dot.SetParent(parentObject.transform);
        }
    }
    public void DebugGameObjectCoordinates(List<Vector3> listOfDots, string message)
    {
        foreach (var dotPosition in listOfDots)
        {
            UnityEngine.Debug.Log("" + message + dotPosition);
        }
    }
    public List<Vector3> GetListOfDotsCoordinates()
    {
        List<Vector3> listOfDots = new List<Vector3>();
        foreach (var dotWithText in PenTool.listOfDotsWithText)
        {
            Transform dot = dotWithText.transform.Find("Dot");
            listOfDots.Add(dot.transform.position);          
        }
        return listOfDots;
    }
    public Vector3 CalculateCenterOfGameObjects(List<Vector3> listOfDots)
    {
        Vector3 center = Vector3.zero;
        foreach (var dotPosition in listOfDots)
        {
            center += dotPosition;
        }
        center /= listOfDots.Count;

        return center;
    }
    void Update()
    {
        
    }
}
