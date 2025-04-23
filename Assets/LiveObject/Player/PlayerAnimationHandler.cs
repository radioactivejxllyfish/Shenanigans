using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private Animator animator;
    private float _velocity;
    public Animator headAnimator;
    public Animator torsoAnimator;
    public bool combatMode;
    public Ult_CombatMode ult_CombatMode;

    public string Direction;

    private CursorController _cursor;
    private string currentState;
    private bool ModeSwitching = false;
    
    private bool engage = false;
    private bool disengage = false;

    void Start()
    {
        _cursor = GameObject.FindGameObjectWithTag("Cursor").GetComponent<CursorController>();
        animator = GetComponent<Animator>();
        playerRb = GetComponentInParent<Rigidbody2D>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        HeadAnimation();
        if (!playerController.isDead && !playerController.isStunned)
        {
            Flip();
            if (ult_CombatMode.combatMode)
            {
                if (!engage)
                {
                    StartCoroutine("SwitchMode");
                    combatMode = true;
                    StartCoroutine("CombatEngage");
                    engage = true;
                    disengage = false; // reset the other flag
                }
            }
            else
            {
                if (!disengage)
                {
                    StartCoroutine("SwitchMode");
                    combatMode = false;
                    StartCoroutine("CombatDisengage");
                    disengage = true;
                    engage = false; // reset the other flag
                }
            }
        }
        _velocity = playerRb.velocity.magnitude;
    }

    private IEnumerator SwitchMode()
    {
        ModeSwitching = true;
        yield return new WaitForSeconds(3f);
        ModeSwitching = false;
    }

    private IEnumerator CombatEngage()
    {
        HeadChangeAnimationState("Head_CombatEngage");
        yield return new WaitForSeconds(1.20f);
        HeadChangeAnimationState("Head_Combat");
    }
    
    private IEnumerator CombatDisengage()
    {
        HeadChangeAnimationState("Head_CombatDisengage");
        yield return new WaitForSeconds(1.20f);
        HeadChangeAnimationState("Head_Idle");
    }

    private void Flip()
    {
        if (!playerController.isDashing && !playerController.isStunned)
        {
            if (_cursor.Direction == "Right")
            {
                Direction = "Right";
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (_cursor.Direction == "Left")
            {
                Direction = "Left";
                transform.localScale = new Vector3(-1 , 1, 1);
            }
        }
    }

    private void HeadAnimation()
    {
        if (combatMode && !playerController.isDead)
        {
        }
        else if (!playerController.isDead && !combatMode && !ModeSwitching)
        {
            HeadChangeAnimationState("Head_Idle");
        }
        else if (playerController.isDead)
        {
            HeadChangeAnimationState("Head_Death");
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
    
    public void HeadChangeAnimationState(string newState)
    {
        string thisState;
        // stop the animation from interrupting itself
        if (currentState == newState) return;
        // play animation
        headAnimator.Play(newState); 
        // reassign current state   
        thisState = newState;
    }
    
    public void TorsoChangeAnimationState(string newState)
    {
        string thisState;
        // stop the animation from interrupting itself
        if (currentState == newState) return;
        // play animation
        headAnimator.Play(newState); 
        // reassign current state   
        thisState = newState;
    }
}
