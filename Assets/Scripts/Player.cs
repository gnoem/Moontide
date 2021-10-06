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
    public int isFacingForward = 1;
    public int rotationOffset = 0;
    public Rigidbody rigidBody;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal == Vector3.up)
            isGrounded = true;
    }

    // Update is called once per frame
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
            isFacingForward = (z > 0) ? 1 : -1;
            rotationOffset = (isFacingForward == 1) ? 0 : 180;

        rigidBody.transform.Rotate(0, x * isFacingForward, 0);
        Vector3 transformForward = isFacingForward == 1 ? transform.forward : -transform.forward;
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
