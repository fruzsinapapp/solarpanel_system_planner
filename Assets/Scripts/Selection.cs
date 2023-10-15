using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{
    public static GameObject GlobalGameObject;

    private Material selectionMaterial;
    private Material originalMaterial;

    private GameObject selectedObject;
    private RaycastHit rayCastHit;

    private bool selectionIsActive;
    public void SelectDotFromTheList()
    {
        SelectDot(PenTool.listOfDotsWithText[0].gameObject);
        //GlobalGameObject = PenTool.listOfDots[0];
    }
    public void SelectDot(GameObject dotWithTextToSelect)
    {
        Transform dotToSelect = dotWithTextToSelect.transform.Find("Dot");
        selectionMaterial = Resources.Load<Material>("SelectedMaterial");
        dotToSelect.GetComponent<MeshRenderer>().material = selectionMaterial;
        if(GlobalGameObject!= null && GlobalGameObject != dotToSelect)
        {
            UnSelectDot(GlobalGameObject);
        }
        GlobalGameObject = dotWithTextToSelect.gameObject;
    }
    public void UnSelectDot(GameObject dotWithTextToSelect)
    {
        Transform dotToSelect = dotWithTextToSelect.transform.Find("Dot");
        originalMaterial = Resources.Load<Material>("OriginalMaterial");
        dotToSelect.GetComponent<MeshRenderer>().material = originalMaterial;
        GlobalGameObject = null;

    }
    void Update()
    {
        /*
        //Select by clicking
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(ray, out rayCastHit))
            {
                if (selectionIsActive && rayCastHit.transform.CompareTag("Selectable"))
                {
                    //This is needed so other can be selected
                    UnSelectDot(selectedObject);
                    selectedObject = null;
                    selectionIsActive = false;
                }
                if (!selectionIsActive && rayCastHit.transform.CompareTag("Selectable"))
                {
                    selectedObject = rayCastHit.transform.gameObject;
                    selectionIsActive = true;
                    SelectDot(selectedObject.gameObject);
                }
            }
        }
        */
    }
}
