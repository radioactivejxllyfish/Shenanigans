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
    public Slider healthBar;
    public PlayerAnimationHandler animatorHandler;
    public CursorController _cursorController;
    public bool isStunned = false;
    public bool ShadowStriking;
    public string dashDir = "LeftF";
    public string walkDir = "LeftF";


    public GameObject grenade;
    public int grenadeCount;
    private void Start()
    {
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        _cursorController = cursor.GetComponent<CursorController>();
        playerRb = GetComponent<Rigidbody2D>();
        _dashpower = 8f;
        speed = 5f;
        maxHealth = 100f;
        health = maxHealth;
        StartCoroutine("Sprint");
        StartCoroutine("HpRegen");
        hasDashed = false;
        grenadeCount = 3;
    }
    
    private void Update()
    {
        MovementInput();
        _staminaBar.value = stamina / MAX_STAMINA;
        healthBar.value = health / maxHealth;
        Dash();
        Move();
        OnDeath();
        ThrowGrenade();
    }


    private void ThrowGrenade()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isStunned && grenadeCount > 0)
        {
            Instantiate(grenade, transform.position, transform.rotation);
        }
    }

    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) && canSprint && !isStunned && movement != Vector3.zero)
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
    public void Move()
    {
        if (!isDashing)
        {
            xAxis = Input.GetAxisRaw("Horizontal");
            yAxis = Input.GetAxisRaw("Vertical");
        }
        WalkDirection();
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
            if (walkDir == "LeftF")
            {
                animatorHandler.ChangeAnimationState("Walk");
            }
            else if (walkDir == "LeftB")
            {
                animatorHandler.ChangeAnimationState("BackwardsWalk");
            }
            else if (walkDir == "RightF")
            {
                animatorHandler.ChangeAnimationState("Walk");
            }
            else if (walkDir == "RightB")
            {
                animatorHandler.ChangeAnimationState("BackwardsWalk");
            }
        }
        else if (playerRb.velocity.magnitude <= 0.2f && !ShadowStriking && !isDashing)
        {
            animatorHandler.ChangeAnimationState("Idle");
        }
        else if (sprinting && !isDashing)
        {
            if (walkDir == "LeftF")
            {
                animatorHandler.ChangeAnimationState("Run");
            }
            else if (walkDir == "LeftB")
            {
                animatorHandler.ChangeAnimationState("BackwardsRun");
            }
            else if (walkDir == "RightF")
            {
                animatorHandler.ChangeAnimationState("Run");
            }
            else if (walkDir == "RightB")
            {
                animatorHandler.ChangeAnimationState("BackwardsRun");
            }
        }
        else if (ShadowStriking && !isDashing)
        {
            animatorHandler.ChangeAnimationState("ShadowStrike");
        }
        else if (isDashing)
        {
            if (dashDir == "LeftF")
            {
                animatorHandler.ChangeAnimationState("Roll");
            }
            else if (dashDir == "LeftB")
            {
                animatorHandler.ChangeAnimationState("BackwardsRoll");
            }
            else if (dashDir == "RightF")
            {
                animatorHandler.ChangeAnimationState("Roll");
            }
            else if (dashDir == "RightB")
            {
                animatorHandler.ChangeAnimationState("BackwardsRoll");
            }
            else if (dashDir == "Roll")
            {
                animatorHandler.ChangeAnimationState("Roll");
            }
        }
        
        
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void OnDeath()
    {
        if (health <= 0)
        {
            animatorHandler.ChangeAnimationState("Death");
            Destroy(gameObject, 3f);
        }
    }

    private void RollDirection()
    {

            if (movement.normalized.x < 0 && _cursorController.Direction == "Left")
            {
                dashDir = "LeftF";
            }
            else if (movement.normalized.x > 0 && _cursorController.Direction == "Left")
            {
                dashDir = "LeftB";
            }
            else if (movement.normalized.x > 0 && _cursorController.Direction == "Right")
            {
                dashDir = "RightF";
            }
            else if (movement.normalized.x < 0 && _cursorController.Direction == "Right")
            {
                dashDir = "RightB";
            }
            else if (movement.normalized.x == 0)
            {
                walkDir = "Roll";
            }
    }
    
    private void WalkDirection()
    {

        if (movement.normalized.x < 0 && _cursorController.Direction == "Left")
        {
            walkDir = "LeftF";
        }
        else if (movement.normalized.x > 0 && _cursorController.Direction == "Left")
        {
            walkDir = "LeftB";
        }
        else if (movement.normalized.x > 0 && _cursorController.Direction == "Right")
        {
            walkDir = "RightF";
        }
        else if (movement.normalized.x < 0 && _cursorController.Direction == "Right")
        {
            walkDir = "RightB";
        }
        else if (movement.normalized.x == 0)
        {
            walkDir = "RightF";
        }
    }


    
    private IEnumerator Sprint()
    {
        while (true)
        {
            if (sprinting && stamina > 0 )
            {
                speed = 9f;
                stamina -= 2f;
                if (stamina < 0) stamina = 0;
            }
            else if (!sprinting)
            {
                speed = 5f;
                stamina += 1f;
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

    private void StunTrigger()
    {
        isStunned = true;
    }
    private IEnumerator Dash()
    {
        RollDirection();
        Vector2 direction = movement.normalized;
        yield return new WaitForSeconds(0.01f);
        canDash = false;
        isDashing = true;
        stamina -= 25;
        float duration = 0.6f;
        float elapsed = 0.0f;
        Invoke("StunTrigger", 0.6f);
        while (elapsed < duration)
        {
            playerRb.AddForce(direction * _dashpower, ForceMode2D.Impulse);
            elapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1.1f);
        isDashing = false;
        canDash = true;
        isStunned = false;
    }

    private IEnumerator HpRegen()
    {
        while (true)
        {
            if (health < maxHealth)
            {
                yield return new WaitForSeconds(2.5f);
                while (health < maxHealth)
                {
                    health += Time.deltaTime;
                    yield return new WaitForSeconds(0.02f);
                }
            }

            yield return null;
        }

    }
}
