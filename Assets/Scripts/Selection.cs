using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{
    public static GameObject GlobalGameObject;

    public Material selectionMaterial;
    public Material originalMaterial;

    private GameObject selectedObject;
    private RaycastHit rayCastHit;

    private bool selectionIsActive;

    static public void SelectDotFromTheList()
    {
        GlobalGameObject = PenTool.listOfDots[0];
    }
    public void SelectDot(GameObject dotToSelect)
    {
        Debug.Log(dotToSelect.GetComponent<MeshRenderer>().material);
        dotToSelect.GetComponent<MeshRenderer>().material = selectionMaterial;

        GlobalGameObject = dotToSelect.gameObject;
    }
    public void UnSelectDot(GameObject dotToSelect)
    {
        dotToSelect.GetComponent<MeshRenderer>().material = originalMaterial;
        GlobalGameObject = null;

    }
    void Update()
    {
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
    }
}
