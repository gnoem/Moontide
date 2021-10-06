using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Player target;
    private Vector3 targetPosition => target.transform.position;
    public Vector3 offset;

    void Update()
    {
        int fac = target.isFacingForward;
        int rotationOffset = fac == 1 ? 0 : 180;
        Vector3 transformOffset = Vector3.Scale(offset, new Vector3(1, 1, fac));
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, target.transform.eulerAngles.y + rotationOffset, transform.eulerAngles.z);
        // convert targetPosition to localSpace and add offset along -Vector3.forward
        Vector3 relativePosition = target.transform.InverseTransformPoint(targetPosition) + transformOffset;
        // convert back to world space = that's our transform.position
        transform.position = target.transform.TransformPoint(relativePosition);
    }
}
