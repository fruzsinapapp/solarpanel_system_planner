using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{
    public static GameObject GlobalGameObject;

    public Material selectionMaterial;
    public  Material originalMaterial;

    private Transform selection;
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
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out rayCastHit))
            {
                selection = rayCastHit.transform;
                
                if (selection.CompareTag("Selectable"))
                {
                    selection.GetComponent<MeshRenderer>().material = selectionMaterial;
                    selectionIsActive = true;
                }
                else
                {
                    selection = null;
                }
            }
        }
        if(selection!= null)
        {
            GlobalGameObject = selection.gameObject;  
        }   
    }
}
