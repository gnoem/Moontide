using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // how fast do we want to move
    public float moveSpeed = 5;
    public float turnSpeed = 2;
    public float jumpForce = 5;
    private bool isGrounded;
    public Vector3 direction = Vector3.forward; // 1 = forwards, -1 = backwards
    private Vector3 prevDirection = Vector3.forward;
    public bool isFacingForward => direction.z == 1;
    public Rigidbody rigidBody;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal == Vector3.up)
            isGrounded = true;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal") * turnSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;

        // are we walking forward or backward
        if (z != 0)
        {
            prevDirection = direction;
            direction = (z > 0) ? Vector3.forward : -Vector3.forward;
        }

        int shouldSwivel = (prevDirection.z != direction.z) ? 180 : 0;
        Vector3 transformForward = (isFacingForward) ? transform.forward : -transform.forward;
        
        rigidBody.transform.Rotate(0, (x + shouldSwivel) * direction.z, 0);
        rigidBody.velocity = transformForward * z;
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
