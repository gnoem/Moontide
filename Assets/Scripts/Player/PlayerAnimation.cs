using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    public Player player;
    void Start()
    {
        // initialize animator
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        animator.SetBool("isWalking", player.isWalking);
    }
}
