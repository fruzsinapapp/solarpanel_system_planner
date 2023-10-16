using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZeroMarkerScript : MonoBehaviour
{
    [SerializeField] private GameObject zeroMarkerDotPrefab;
    [SerializeField] Transform zeroMarkerParent;

    public static GameObject zeroMarkerAssigned;

    void Start()
    {
        //zeroMarkerDot = Resources.Load<GameObject>("ZeroMarker");
        GameObject zeroMarkerDotInst = Instantiate(zeroMarkerDotPrefab, Vector3.zero, Quaternion.identity, zeroMarkerParent);
        
        Transform dot = zeroMarkerDotInst.transform.Find("ZeroDot");

        Transform coordinatesText = zeroMarkerDotInst.transform.Find("DotCoordinatesText");
        TextMeshPro myTextMeshPro = coordinatesText.GetComponent<TextMeshPro>();




        myTextMeshPro.text = "x: " + (dot.transform.position.x * 100f).ToString("N2") + "\ny: " + (dot.transform.position.y * 100f).ToString("N2") + "\nz: " + (dot.transform.position.z * 100f).ToString("N2");
        zeroMarkerAssigned = zeroMarkerDotInst;
    }

    private float updateInterval = 0.1f;
    private float timeSinceLastUpdate = 0;

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= updateInterval)
        {
            if(zeroMarkerAssigned != null)
            {
                Transform coordinatesText = zeroMarkerAssigned.transform.Find("DotCoordinatesText");
                TextMeshPro myTextMeshPro = coordinatesText.GetComponent<TextMeshPro>();
                Transform dot = zeroMarkerAssigned.transform.Find("ZeroDot");
                myTextMeshPro.text = "x: " + (dot.transform.position.x * 100f).ToString("N2") + "\ny: " + (dot.transform.position.y * 100f).ToString("N2") + "\nz: " + (dot.transform.position.z * 100f).ToString("N2");

            }

            timeSinceLastUpdate = 0;
        }
    }

    public Vector3 UseRealCoordinates(Transform dotWithOldOrigo)
    {
        Transform dot = zeroMarkerAssigned.transform.Find("ZeroDot");

        Vector3 offset = dotWithOldOrigo.position - dot.transform.position;

        return offset;
    }

    public Vector3 UseRealCoordinatesVector(Vector3 oldVector)
    {
        Transform dot = zeroMarkerAssigned.transform.Find("ZeroDot");

        Vector3 offset = oldVector + dot.transform.position;

        return offset;
    }
}
