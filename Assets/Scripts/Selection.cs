using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{
    public static GameObject GlobalGameObject;

    public Material selectionMaterial;
    public  Material originalMaterial;

    private Transform selectedObject;
    private RaycastHit rayCastHit;

    private bool selectionIsActive;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Unselect
            /*
            if (selection != null)
            {
                selection.GetComponent<MeshRenderer>().material = originalMaterial;
            }
            */
            if (Physics.Raycast(ray, out rayCastHit))
            {
                if (selectionIsActive && rayCastHit.transform.CompareTag("Selectable"))
                {
                    selectedObject.GetComponent<MeshRenderer>().material = originalMaterial;
                    GlobalGameObject = null;
                    selectionIsActive = false;
                    selectedObject= null;
                }
                if (!selectionIsActive && rayCastHit.transform.CompareTag("Selectable"))
                {
                    selectedObject = rayCastHit.transform;

                    if (selectedObject.CompareTag("Selectable"))
                    {
                        selectedObject.GetComponent<MeshRenderer>().material = selectionMaterial;
                        selectionIsActive = true;
                    }
                    else
                    {
                        selectedObject = null;
                    }
                }

            }
        }
        if(selectedObject!= null)
        {
            GlobalGameObject = selectedObject.gameObject;  
        }
        else
        {

        }
    }
}
