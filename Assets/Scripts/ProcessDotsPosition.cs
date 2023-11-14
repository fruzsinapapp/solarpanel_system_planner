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

        List<Vector3> listOfDots = GetListOfDots();
        Vector3 center = CalculateCenterOfGameObjects(listOfDots);
        UnityEngine.Debug.Log("[CENTER] Original center: " + center);
    }
    public List<Vector3> GetListOfDots()
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
