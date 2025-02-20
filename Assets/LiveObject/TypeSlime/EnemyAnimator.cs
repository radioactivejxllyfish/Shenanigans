using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : EnemyTypeSlime
{
    private const string IDLE = "idle";
    private const string WALK = "move";
    private const string DIE = "death";

    private string currentState;

    void Update()
    {
        Animate();
    }

    private void Animate()
    {
        if (isAlive && !isStunned && rigidBody.velocity.magnitude >= 0.3f)
        {
            ChangeAnimationState(WALK);
        }
        else if (isAlive && !isStunned && rigidBody.velocity.magnitude <= 0.3f)
        {
            ChangeAnimationState(IDLE);
        }
        else if (!isAlive)
        {
            ChangeAnimationState(DIE);
        }
    }
    
    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }
}