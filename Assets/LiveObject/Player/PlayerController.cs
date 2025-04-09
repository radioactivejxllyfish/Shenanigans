using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : PlayerVarPool
{
    public bool usingItem = false;
    public bool usingSkill;

    public float Multiplier_MeleeDamage;
    public float Multiplier_RangedDamage;
    public float Multiplier_SkillDamage;
    public float Multiplier_UltimateDamage;

    public float Resistance_Melee;
    public float Resistance_Ranged;
    public float Resistance_Explosive;
    public float Resistance_Energy;

    public float _dashpower;

    public string dashDir = "Roll";
    public string walkDir = "LeftF";
    
    public bool canDash = true;
    public bool hasDashed = false;
    public bool sprinting;
    public bool isDashing = false;
    public bool isMoving;
    public bool isStunned = false;
    public bool isDead = false;

    public Stats playerStats;
    public PlayerAnimationHandler animatorHandler;
    public CursorController _cursorController;
    public AudioSource source;
    public GameObject grenade;



    float elapsed = 0f;

    public float footstepInterval = 0.5f;
    public AudioClip walk1;
    public AudioClip walk2;
    public AudioClip walk3;
    public AudioClip walk4;
    public AudioClip dashBackwards;
    public AudioClip dashBackwards2;
    public AudioClip dash;

    public bool underUI = false;
    public float currentSpeed;
    public float slowSpd;
    public int grenadeCount;
    
    public string Class;
    
    private float resistance;

    private void Start()
    {
        Class = "U11_SpecialRecon";
        
        playerStats = gameObject.AddComponent<Stats>();
        if (playerStats != null)
        {
            cameraSmoother = Camera.main.GetComponent<CameraSmoother>();
            source = GetComponent<AudioSource>();
            cursor = GameObject.FindGameObjectWithTag("Cursor");
            _cursorController = cursor.GetComponent<CursorController>();
            playerRb = GetComponent<Rigidbody2D>();
            StartCoroutine("Sprint");
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        stamina = Mathf.Clamp(stamina,0, MAX_STAMINA);
        if (!isDead)
        {
            SoundFootstep();
            MovementInput();
            ThrowGrenade();
            Move();
        }
        else
        {
            playerRb.velocity = Vector2.zero;
        }
        OnDeath();
    }
    
    public void TakeDamage(float damage, string type)
    {
        switch (type)
        {
            case "Melee":
                resistance = Resistance_Melee;
                break;
            case "Ranged":
                resistance = Resistance_Ranged;
                break;
            case "Explosive":
                resistance = Resistance_Explosive;
                break;
            case "Energy":
                resistance = Resistance_Energy;
                break;
        }
                
        if (health > 0)
        {
            stamina -= damage * 1.85f;
            float deduction;
            if (armorCount > 0)
            {
                deduction = damage - armorCount;
                armorCount = armorCount - damage;
                if (armorCount < 0)
                {
                    health -= deduction;
                }
            }
            else
            {
                health -= damage * resistance;
            }
            cameraSmoother.CameraShake(0.05f * damage * resistance,0.01f);
        }
    }


    private void ThrowGrenade()
    {
        if (Input.GetKeyDown(KeyCode.G) && !isStunned && grenadeCount > 0)
        {
            grenadeCount -= 1;
            Instantiate(grenade, transform.position, transform.rotation);
        }
    }

    private void MovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isDashing && canSprint &&!usingSkill && !isStunned && movement != Vector3.zero)
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && sprinting && !hasDashed && stamina >= 35 && canDash && !isStunned && !usingSkill && movement != Vector3.zero)
        {
            StartCoroutine("Dash");
            if (dashDir == "LeftF" || dashDir == "RightF" || dashDir == "Roll")
            {
                source.clip = dash;
            }
            else
            {
                source.clip = dashBackwards;
            }

            StartCoroutine("PlaySound");
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
        
        if (isStunned || usingSkill)
        {
            source.Stop();
            playerRb.velocity = Vector2.zero;
        }
        else
        {
            playerRb.velocity = movement.normalized * currentSpeed;
        }

        if (playerRb.velocity != Vector2.zero && !isDashing)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        if (playerRb.velocity.magnitude >= 0.4f && !isDashing && !sprinting)
        {
            if (walkDir == "LeftF")
            {
                animatorHandler.ChangeAnimationState("Walk");
            }
            else if (walkDir == "LeftB")
            {
                animatorHandler.ChangeAnimationState("Walk_Backwards");
            }
            else if (walkDir == "RightF")
            {
                animatorHandler.ChangeAnimationState("Walk");
            }
            else if (walkDir == "RightB")
            {
                animatorHandler.ChangeAnimationState("Walk_Backwards");
            }
        }
        else if (playerRb.velocity.magnitude <= 0.2f && !usingSkill && !isDashing)
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
                animatorHandler.ChangeAnimationState("Run_Backwards");
            }
            else if (walkDir == "RightF")
            {
                animatorHandler.ChangeAnimationState("Run");
            }
            else if (walkDir == "RightB")
            {
                animatorHandler.ChangeAnimationState("Run_Backwards");
            }
        }
        else if (isDashing)
        {
            if (dashDir == "LeftF")
            {
                animatorHandler.ChangeAnimationState("Roll");
            }
            else if (dashDir == "LeftB")
            {
                animatorHandler.ChangeAnimationState("Roll_Backwards");
            }
            else if (dashDir == "RightF")
            {
                animatorHandler.ChangeAnimationState("Roll");
            }
            else if (dashDir == "RightB")
            {
                animatorHandler.ChangeAnimationState("Roll_Backwards");
            }
            else if (dashDir == "Roll")
            {
                animatorHandler.ChangeAnimationState("Roll");
            }
        }
    }

    

    public void OnDeath()
    {
        if (health <= 0)
        {
            animatorHandler.ChangeAnimationState("Die");
            isDead = true;
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
                currentSpeed = 9f;
                stamina -= 2f;
                if (stamina < 0) stamina = 0;
                yield return null;
            }
            else if (!sprinting)
            {
                currentSpeed = 5f;
                stamina += 1f;
                if (stamina > MAX_STAMINA) stamina = MAX_STAMINA;
                yield return null;
            }
            else if (stamina == 0)
            {
                canSprint = false;
                currentSpeed = 3f;
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
        float duration = 1.15f;
        float elapsed = 0.0f;
        float stunTime;
        float actualDashPower = _dashpower;
        RollDirection();

        if (dashDir == "LeftF" || dashDir == "RightF" || dashDir == "Roll")
        {
            actualDashPower = _dashpower;
            stunTime = 0.1f;
        }
        else 
        {
            actualDashPower = _dashpower * 1.3f;
            duration = 0.85f;
            stunTime = 1f;
        }

        Vector2 direction = movement.normalized;
        yield return new WaitForSeconds(0.01f);
        canSprint = false;
        canDash = false;
        isDashing = true;
        stamina -= 25;

        Invoke("StunTrigger", duration);
        while (elapsed < duration)
        {
            actualDashPower -= 0.1f;
            playerRb.AddForce(direction * actualDashPower, ForceMode2D.Impulse);
            elapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(stunTime);

        isDashing = false;
        canDash = true;
        isStunned = false;
        yield return new WaitForSeconds(0.3f);
        canSprint = true;

    }


    private void SoundFootstep()
    {
        
        if (isMoving && !isStunned && !isDead)
        {
            if (sprinting)
            {
                footstepInterval = 0.30f;
            }
            else
            {
                footstepInterval = 0.46f;
            }
            int x = Random.Range(1, 5);

            if (elapsed < footstepInterval)
            {
                elapsed += Time.deltaTime;
            }
            else
            {
                elapsed = 0;
                if (x == 1)
                {
                    source.clip = walk1;
                }
                else if (x == 2)
                {
                    source.clip = walk2;
                }
                else if (x == 3)
                {
                    source.clip = walk2;
                }
                else
                {
                    source.clip = walk4;
                }
                
                source.Play();
            }
        }
        else
        {
            elapsed = 0;
        }
    }



    private IEnumerator PlaySound()
    {
        bool played = false;
        if (!played)
        {
            source.Play();
            played = true;
        }
        yield return null;
    }

    public void UseItemCountdown(float duration, string animation)
    {
        
    }

    private IEnumerator UseItem(float time)
    {
        yield return null;
    }
}
