using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionCode : MonoBehaviour
{
    public static float originalZPosition;
    public static float originalAngleTwoDots;

    #region Testing (called from GetPositionsButton.cs)
    public void ProjectionCodeImpl(Vector3 dotToBeProjected)
    {
        Vector3 projectionDirection = new Vector3(0, 0, 1);

        float zPosition = dotToBeProjected.z;

        dotToBeProjected = ProjectionOntoPlane(dotToBeProjected, projectionDirection);
        float originalAngle = Vector3.Angle(dotToBeProjected, projectionDirection);

        Debug.Log("X: " + dotToBeProjected.x + ", Y: " + dotToBeProjected.y);
        Debug.Log("Angle: " + originalAngle);
    }
    #endregion
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
        GameObject dot1 = PenTool.listOfDotsWithText[0].gameObject.GetComponent<GameObject>();
        GameObject dot2 = PenTool.listOfDotsWithText[1].gameObject.GetComponent<GameObject>();
        Vector3 vectorBetweenDots = dot1.transform.position - dot2.transform.position;
        Vector3 projectionDirection = new Vector3(0, 0, 1);

        originalAngleTwoDots = Vector3.Angle(vectorBetweenDots, projectionDirection);
        originalZPosition = dotToBeProjected.z;

        dotToBeProjected = ProjectionOntoPlane(dotToBeProjected, projectionDirection);

        return dotToBeProjected;
    }
}
