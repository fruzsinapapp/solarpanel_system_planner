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

public class PenTool : MonoBehaviour
{
    [Header("Dots")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] Transform dotParent;

    [Header("Lines")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] Transform lineParent;
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
    private void Update()
    {
        testing();
    }
    public void testing()
    {
        foreach (var p in pointerDataToUpdate)
        {
            PointerData pointerData = p.Value;
            IMixedRealityPointer pointer = pointerData.pointer;
            Debug.Log(pointer.Rays);
            if (pointer.IsInteractionEnabled
                && pointer.Rays != null
                && pointer.Rays.Length > 0
                && Input.GetKeyDown(KeyCode.LeftAlt))
            {

                Debug.Log("HELLO RAY");
            }

        }
    }
    public void SpawnNewDot()
    {
        if (currentLine == null)
        {
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent).GetComponent<LineController>();
        }

        GameObject dot = Instantiate(dotPrefab, GetMousePosition(), Quaternion.identity, dotParent);
        Debug.Log("X:" + GetMousePosition().x);
        Debug.Log("Y:" + GetMousePosition().y);
        Debug.Log("Z:" + GetMousePosition().z);
        currentLine.AddPoint(dot.transform);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePosition.x += 2;
        worldMousePosition.z += 5;

        return worldMousePosition;
    }
}
