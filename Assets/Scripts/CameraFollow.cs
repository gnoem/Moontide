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
        int rotationOffset = (target.direction.z) == 1 ? 0 : 180;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, target.transform.eulerAngles.y + rotationOffset, transform.eulerAngles.z);
        // convert targetPosition to localSpace and add offset along -Vector3.forward
        Vector3 relativePosition = target.transform.InverseTransformPoint(targetPosition) + Vector3.Scale(transformOffset, new Vector3(1, 1, target.direction.z));
        // convert back to world space = that's our transform.position
        transform.position = target.transform.TransformPoint(relativePosition);
    }
}
