using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // how fast do we want to move
    public float moveSpeed = 5;
    public float turnSpeed = 2;
    public float swivelSpeed = 10;
    public float slopeLimit = 45f;
    private bool isGrounded;
    private bool walkingIsPermitted;
    public bool isWalking;
    public bool isSwivelling;
    private float startSwivel;
    private float amountSwivelled = 0;
    public bool faceCameraToStart = true;
    private Vector3 direction;
    private Vector3 prevDirection;
    public bool isFacingForward => direction.z == 1;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        direction = (faceCameraToStart) ? -Vector3.forward : Vector3.forward;
    }
    void Update()
    {
        CheckIfTooSteepToWalk();
        if (walkingIsPermitted)
        {
            HandleMovement();
        }
    }
    private void CheckIfTooSteepToWalk()
    {
        walkingIsPermitted = false;
        float capsuleHeight = Mathf.Max(capsuleCollider.radius * 2f, capsuleCollider.height);
        Vector3 capsuleBottom = transform.TransformPoint(capsuleCollider.center - Vector3.up * capsuleHeight / 2f);
        float radius = transform.TransformVector(capsuleCollider.radius, 0f, 0f).magnitude;

        Ray ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radius * 5f))
        {
            float normalAngle = Vector3.Angle(hit.normal, transform.up);
            if (normalAngle < slopeLimit)
            {
                float maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
                if (hit.distance < maxDist)
                    walkingIsPermitted = true;
            }
        }
    }
    private void StartSwivel()
    {
        if (isSwivelling)
            return;
        
        isWalking = false;
        isSwivelling = true;
        startSwivel = rigidBody.transform.eulerAngles.y;
        amountSwivelled = 0;
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal") * turnSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;

        // are we walking forward or backward
        if (z != 0)
        {
            // save the last frame's direction
            prevDirection = direction;
            // update the current direction based on vertical input (up/down arrow keys)
            direction = (z > 0) ? Vector3.forward : -Vector3.forward;
            // handle switching between walking forward & backward
            if (prevDirection.z != direction.z)
                StartSwivel();
            else if (!isSwivelling)
                isWalking = true;
        }
        else
            isWalking = false;

        if (isSwivelling)
        {
            amountSwivelled += swivelSpeed;
            rigidBody.transform.Rotate(0, swivelSpeed, 0);
            if (amountSwivelled >= 180)
                isSwivelling = false;
        }
        else
        {
            Vector3 transformForward = (isFacingForward) ? transform.forward : -transform.forward;
            rigidBody.transform.Rotate(0, x * direction.z, 0);
            rigidBody.velocity = transformForward * z;
        }
    }
}
