using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessDotsPosition : MonoBehaviour
{

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
