using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // how fast do we want to move
    public float moveSpeed = 5;
    // reference our rigidbody component because the way we move in Unity is by modifying our rigidbody's velocity so that we move by physics
    public Rigidbody rigidBody;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;

        rigidBody.velocity = new Vector3(x, rigidBody.velocity.y, z);

        Vector3 vel = rigidBody.velocity;
        vel.y = 0;
        
        // only rotate if we're moving
        if (vel.x != 0 || vel.z != 0)
        {
            transform.forward = vel;
        }
    }
}
