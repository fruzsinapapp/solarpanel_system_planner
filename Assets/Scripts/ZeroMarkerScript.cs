using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZeroMarkerScript : MonoBehaviour
{
    [SerializeField] private GameObject zeroMarkerDot;

    void Start()
    {
        Vector3 startPosition = new Vector3(0.5f, 0f, 1f);
        zeroMarkerDot.transform.position = startPosition;
        Transform dot = zeroMarkerDot.transform.Find("Dot");

        Transform coordinatesText = zeroMarkerDot.transform.Find("DotCoordinatesText");
        TextMeshPro myTextMeshPro = coordinatesText.GetComponent<TextMeshPro>();
        myTextMeshPro.text = "x: " + (dot.transform.position.x * 100f).ToString("N2") + "\ny: " + (dot.transform.position.y * 100f).ToString("N2") + "\nz: " + (dot.transform.position.z * 100f).ToString("N2");
        dot.tag = "Selectable";
    }

    private float updateInterval = 0.1f;
    private float timeSinceLastUpdate = 0;

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= updateInterval)
        {
            Transform coordinatesText = zeroMarkerDot.transform.Find("DotCoordinatesText");
            TextMeshPro myTextMeshPro = coordinatesText.GetComponent<TextMeshPro>();
            Transform dot = zeroMarkerDot.transform.Find("Dot");
            myTextMeshPro.text = "x: " + (dot.transform.position.x * 100f).ToString("N2") + "\ny: " + (dot.transform.position.y * 100f).ToString("N2") + "\nz: " + (dot.transform.position.z * 100f).ToString("N2");

            timeSinceLastUpdate = 0;
        }
    }
}
