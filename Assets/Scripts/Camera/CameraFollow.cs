using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Player target;
    private Vector3 targetPosition => target.transform.position;
    public Vector3 transformOffset = new Vector3(0, 12, -20);
    void Update()
    {
        if (!target.isSwivelling)
        {
            LockPositionToTarget();
            LockRotationToTarget();
        }
    }
    void LockPositionToTarget()
    {
        // adjust transformOffset based on the direction the target is facing
        Vector3 offsetDirection = new Vector3(1, 1, target.isFacingForward ? 1 : -1);
        Vector3 adjustedOffset = Vector3.Scale(transformOffset, offsetDirection);
        // convert targetPosition to localSpace and add offset
        Vector3 relativePosition = target.transform.InverseTransformPoint(targetPosition) + adjustedOffset;
        // convert back to world space and set that as our camera's transform.position
        transform.position = target.transform.TransformPoint(relativePosition);
    }
    void LockRotationToTarget()
    {
        // camera's rotation.y should either be the same or the opposite as the target's rotation.y, depending on whether or not the target is facing the camera
        int rotationOffset = target.isFacingForward ? 0 : 180;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, target.transform.eulerAngles.y + rotationOffset, transform.eulerAngles.z);
    }
}
