using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionCode : MonoBehaviour
{
    #region Testing (called from GetPositionsButton.cs)
    public void ProjectionCodeImpl(Vector3 dotToBeProjected)
    {
        Vector3 projectionDirection = new Vector3(0, 0, 1);

        float originalZPosition = dotToBeProjected.z;

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
        Vector3 projectionDirection = new Vector3(0, 0, 1);

        float originalZPosition = dotToBeProjected.z;

        dotToBeProjected = ProjectionOntoPlane(dotToBeProjected, projectionDirection);

        float originalAngle = Vector3.Angle(dotToBeProjected, projectionDirection);

        return dotToBeProjected;
    }
}
