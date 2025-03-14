using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private Animator animator;
    private float _velocity;

    private CursorController _cursor;
    private string currentState;
    void Start()
    {
        _cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<CursorController>();
        animator = GetComponent<Animator>();
        playerRb = GetComponentInParent<Rigidbody2D>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (!playerController.isDead && !playerController.isStunned)
        {
            Flip();

        }
        _velocity = playerRb.velocity.magnitude;
    }

    private void Flip()
    {
        if (!playerController.isDashing)
        {
            if (_cursor.Direction == "Right")
            {
                transform.localScale = new Vector3(1.45f, 1.45f, 1.45f);
            }
            else if (_cursor.Direction == "Left")
            {
                transform.localScale = new Vector3(-1.45f, 1.45f, 1.45f);
            }
        }

    }

    public void ChangeAnimationState(string newState)
    {
        // stop the animation from interrupting itself
        if (currentState == newState) return;
        // play animation
        animator.Play(newState); 
        // reassign current state   
        currentState = newState;
        
    }
}
