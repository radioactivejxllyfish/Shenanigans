using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : PlayerController
{
    private Animator animator;
    private float _velocity;

    private CursorController _cursor;
    private string currentState;
    public const string IDLE = "Idle";
    public const string WALK = "Walk";
    public const string DASH = "Roll";
    public const string RUN = "Run";
    void Start()
    {
        _cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<CursorController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Flip();
        _velocity = playerRb.velocity.magnitude;
    }

    private void Flip()
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
