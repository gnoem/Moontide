using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // how fast do we want to move
    public float moveSpeed = 5;
    public float turnSpeed = 2;
    public float jumpForce = 5;
    public float swivelSpeed = 60;
    private bool isGrounded;
    public bool isSwivelling;
    private float startSwivel;
    private float amountSwivelled = 0;
    public bool faceCameraToStart = true;
    private Vector3 direction;
    private Vector3 prevDirection;
    public bool isFacingForward => direction.z == 1;
    public Rigidbody rigidBody;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal == Vector3.up)
            isGrounded = true;
    }
    void Start()
    {
        direction = (faceCameraToStart) ? -Vector3.forward : Vector3.forward;
    }
    void Update()
    {
        HandleMovement();
        HandleJump();
    }
    private void StartSwivel()
    {
        if (isSwivelling)
            return;
        
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
        }

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

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
