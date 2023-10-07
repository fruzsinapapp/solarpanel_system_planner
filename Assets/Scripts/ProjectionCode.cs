using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionCode : MonoBehaviour
{
    public void ProjectionCodeImpl(Vector3 dotToBeProjected)
    {

        Vector3 projectionDirection = new Vector3(0, 0, 1);

        //Vector2[] projectedCorners = new Vector2[rectangleCorners.Length];
        Vector2 projectedCorner = new Vector2();
        //for (int i = 0; i < rectangleCorners.Length; i++)
        //{
        dotToBeProjected = ProjectionOntoPlane(dotToBeProjected, projectionDirection);
        //}
        //foreach(var dot in projectedCorners)
        //{
            Debug.Log("X: " + dotToBeProjected.x + ", Y: " + dotToBeProjected.y);
        //}
    }

    private Vector2 ProjectionOntoPlane(Vector3 point3D, Vector3 planeNormal)
    {
        planeNormal = Vector3.Normalize(planeNormal);

        float dotProduct = Vector3.Dot(point3D, planeNormal);

        Vector3 projectedPoint3D = point3D - dotProduct * planeNormal;

        Vector2 projectedPoint2D = new Vector2(projectedPoint3D.x, projectedPoint3D.y);

        return projectedPoint2D;
    }

    public Vector2 ProjectionForPython(Vector3 dotToBeProjected)
    {
        Vector3 projectionDirection = new Vector3(0, 0, 1);
        Vector2 projectedCorner = new Vector2();

        dotToBeProjected = ProjectionOntoPlane(dotToBeProjected, projectionDirection);
        return dotToBeProjected;
    }
}
