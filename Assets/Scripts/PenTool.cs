using Microsoft.MixedReality.Toolkit.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using Microsoft.MixedReality.Toolkit;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using TMPro;

public class PenTool : MonoBehaviour
{
    public static ZeroMarkerScript zeroMarkerScript;
    [Header("Dots")]
    [SerializeField] private GameObject dotWithTextPrefab;
    [SerializeField] Transform dotParent;

    [Header("Lines")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] Transform lineParent;

    public static List<GameObject> listOfDotsWithText = new List<GameObject>();

    private void Start()
    {
        zeroMarkerScript = new ZeroMarkerScript();
    }
    protected class PointerData
    {
        public readonly IMixedRealityPointer pointer;

        public Vector3? lastMousePoint3d = null; // Last position of the pointer for the input in 3D.
        public PointerEventData.FramePressState nextPressState = PointerEventData.FramePressState.NotChanged;

        public MouseState mouseState = new MouseState();
        public PointerEventData eventDataLeft;
        public PointerEventData eventDataMiddle; // Middle and right are placeholders to simulate mouse input.
        public PointerEventData eventDataRight;

        public PointerData(IMixedRealityPointer pointer, EventSystem eventSystem)
        {
            this.pointer = pointer;
            eventDataLeft = new PointerEventData(eventSystem);
            eventDataMiddle = new PointerEventData(eventSystem);
            eventDataRight = new PointerEventData(eventSystem);
        }
    }
    private LineController currentLine;
    protected readonly Dictionary<int, PointerData> pointerDataToUpdate = new Dictionary<int, PointerData>();

    public void SpawnNewDot()
    {
        if (currentLine == null)
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent).GetComponent<LineController>();

        Selection s = new Selection();
        GameObject dotWithText = Instantiate(dotWithTextPrefab, GetMousePosition(), Quaternion.identity, dotParent);
        Transform dot = dotWithText.transform.Find("Dot");

        Transform coordinatesText = dotWithText.transform.Find("DotCoordinatesText");
        TextMeshPro myTextMeshPro = coordinatesText.GetComponent<TextMeshPro>();

        //write coordinates with new origo
        Vector3 newCoordinates = zeroMarkerScript.UseRealCoordinates(dot);
        UnityEngine.Debug.Log("New coordinates: " + newCoordinates*100f);
        myTextMeshPro.text = "x: " + (newCoordinates.x * 100f).ToString("N2") + "\ny: " + (newCoordinates.y * 100f).ToString("N2") + "\nz: " + (newCoordinates.z * 100f).ToString("N2");
        dot.tag = "Selectable";
        listOfDotsWithText.Add(dotWithText);
        currentLine.AddPoint(dot.transform);
        s.SelectDotFromTheList();
    }

    private float updateInterval = 0.1f;
    private float timeSinceLastUpdate = 0;

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= updateInterval)
        {
            if (listOfDotsWithText != null)
            {
                foreach (var dotWithText in listOfDotsWithText)
                {
                    Transform coordinatesText = dotWithText.transform.Find("DotCoordinatesText");
                    TextMeshPro myTextMeshPro = coordinatesText.GetComponent<TextMeshPro>();
                    Transform dot = dotWithText.transform.Find("Dot");
                    Vector3 newCoordinates = zeroMarkerScript.UseRealCoordinates(dot);
                    myTextMeshPro.text = "x: " + (newCoordinates.x * 100f).ToString("N2") + "\ny: " + (newCoordinates.y * 100f).ToString("N2") + "\nz: " + (newCoordinates.z * 100f).ToString("N2");
                }
            }
            timeSinceLastUpdate = 0;
        }
    }
    private Vector3 GetMousePosition()
    {
        /*
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePosition.x += 2;
        worldMousePosition.z += 5;

        return worldMousePosition;
        */
        Vector3 zeroPosition = new Vector3 { x = 0.5f, y = 0, z = 1 };

        return zeroPosition;
    }
}
