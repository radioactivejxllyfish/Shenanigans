using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : PlayerController
{
    private Animator animator;
    private bool _isWalking;
    private bool _idle;
    private float _velocity;

    private string currentState;
    const string IDLE = "Idle";
    const string WALK = "Walk";
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        _velocity = playerRb.velocity.magnitude;
    }

    private void Movement()
    {
        if (playerRb.velocity.magnitude > 0.2f)
        {
            ChangeAnimationState(WALK);
            _isWalking = true;
        }
        else
        {
            ChangeAnimationState(IDLE);
            _isWalking = false;
            _idle = true;
        }
    }

    private void ChangeAnimationState(string newState)
    {
        // stop the animation from interrupting itself
        if (currentState == newState) return;
        // play animation
        animator.Play(newState); 
        // reassign current state   
        currentState = newState;
        
    }
}
