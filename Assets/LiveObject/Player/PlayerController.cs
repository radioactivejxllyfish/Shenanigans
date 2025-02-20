using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : PlayerVarPool
{
    public float _dashpower;
    public bool canDash = true;
    public bool hasDashed = false;
    public float stamina = 100;
    public float MAX_STAMINA = 100;
    public bool sprinting;
    public bool isDashing = false;
    public Slider _staminaBar;
    public PlayerAnimationHandler animatorHandler;
    public bool isStunned = false;
    public bool ShadowStriking;
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        _dashpower = 10f;
        speed = 5f;
        health = 100f;
        StartCoroutine("Sprint");
        hasDashed = false;
    }
    
    private void Update()
    {
        _staminaBar.value = stamina;
        Dash();
        Move();
        if (Input.GetKey(KeyCode.LeftShift) && canSprint && !isStunned)
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !hasDashed && stamina >= 35 && canDash && !isStunned && !ShadowStriking && movement != Vector3.zero)
        {
            StartCoroutine("Dash");
        }
    }


    public override void Move()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        movement = new Vector2(xAxis, yAxis);
        if (isStunned || ShadowStriking)
        {
            playerRb.velocity = Vector2.zero;
        }
        else
        {
            playerRb.velocity = movement.normalized * speed;
        }
        if (playerRb.velocity.magnitude >= 0.4f && !isDashing && !sprinting)
        {
            animatorHandler.ChangeAnimationState("Walk");
        }
        else if (isDashing)
        {
            animatorHandler.ChangeAnimationState("Roll");
        }
        else if (playerRb.velocity.magnitude <= 0.2f && !ShadowStriking)
        {
            animatorHandler.ChangeAnimationState("Idle");
        }
        else if (sprinting)
        {
            animatorHandler.ChangeAnimationState("Run");
        }
        else if (ShadowStriking)
        {
            animatorHandler.ChangeAnimationState("ShadowStrike");
        }
        
        
    }


    
    private IEnumerator Sprint()
    {
        while (true)
        {
            if (sprinting && stamina > 0 )
            {
                speed = 9f;
                stamina -= 2;
                if (stamina < 0) stamina = 0;
            }
            else if (!sprinting)
            {
                speed = 5f;
                stamina += 1;
                if (stamina > MAX_STAMINA) stamina = MAX_STAMINA;
            }
            else if (stamina == 0)
            {
                canSprint = false;
                speed = 3f;
                yield return new WaitForSeconds(3f);
            }

            if (!canSprint && stamina > 30)
            {
                canSprint = true;
            }
            yield return new WaitForSeconds(0.1f);
        }

    }

    private void DashAnim()
    {
        isStunned = true;
        animatorHandler.ChangeAnimationState("Roll");
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        Vector2 guh = new Vector2(movement.x, movement.y);
        stamina -= 25;
        float duration = 0.5f;
        float elapsed = 0.0f;
        Invoke("DashAnim", 0.6f);
        while (elapsed < duration)
        {
            playerRb.AddForce(guh * _dashpower, ForceMode2D.Impulse);
            elapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.7f);
        isDashing = false;
        canDash = true;
        isStunned = false;


    }
    
    
}
