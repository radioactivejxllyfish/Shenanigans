using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PlayerController : PlayerVarPool
{
    public bool usingItem;
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
    public bool hasDashed;
    public bool sprinting;
    public bool isDashing;
    public bool isMoving;
    public bool isStunned;
    public bool isDead;
    
    private Vector2 lastPosition;
    private Vector2 actualVelocity;

    public Stats playerStats;
    public PlayerAnimationHandler animatorHandler;
    public CursorController _cursorController;
    public AudioSource source;
    public GameObject grenade;


    private float elapsed;

    public float footstepInterval = 0.5f;
    public AudioClip walk1;
    public AudioClip walk2;
    public AudioClip walk3;
    public AudioClip walk4;
    public AudioClip dashBackwards;
    public AudioClip dashBackwards2;
    public AudioClip dash;

    public bool underUI;
    public float currentSpeed;
    public float slowSpd;
    public int grenadeCount;

    public string Class;

    private float resistance;

    public float ultMeter;

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

        ultMeter = 100;
    }

    private void Awake()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            gameObject.transform.DOShakeRotation(1f);
            gameObject.transform.DOKill();
        }

        health = Mathf.Clamp(health, 0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, MAX_STAMINA);
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

    private void FixedUpdate()
    {
        PositionCalculation();
        
        if (actualVelocity.magnitude > 0.01f) {
            // The object is actually moving
            Debug.Log("Object is moving");
        } else {
            // The object is stationary (or barely moving)
            Debug.Log("Object is NOT moving");
        }
    }

    private void PositionCalculation()
    {
        Vector2 currentPosition = transform.position;
        actualVelocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;
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
                if (armorCount < 0) health -= deduction;
            }
            else
            {
                health -= damage * resistance;
            }

            cameraSmoother.CameraShake(0.05f * damage * resistance, 0.01f);
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
        if (Input.GetKey(KeyCode.LeftShift) && !isDashing && canSprint && !usingSkill && !isStunned &&
            movement != Vector3.zero)
            sprinting = true;
        else
            sprinting = false;
        
        if (dashDir == "RightF" || dashDir == "LeftF")
        {
            ForwardRoll();
        }
        
    }

    public void ForwardRoll()
    {
        if (Input.GetKeyDown(KeyCode.Space) && sprinting && !hasDashed && stamina >= 35 && canDash && !isStunned &&
            !usingSkill && movement != Vector3.zero)
        {
            if (playerRb.velocity.x > 0 && _cursorController.Direction == "Right")
            {
                StartCoroutine("Dash");
                source.clip = dash;
                StartCoroutine("PlaySound");
            }
            if (playerRb.velocity.x < 0 && _cursorController.Direction == "Left")
            {
                StartCoroutine("Dash");
                source.clip = dash;
                StartCoroutine("PlaySound");
            }
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
            isMoving = true;
        else
            isMoving = false;

        if (playerRb.velocity.magnitude >= 0.4f && !isDashing && !sprinting)
        {
            if (walkDir == "LeftF")
                animatorHandler.ChangeAnimationState("Walk");
            else if (walkDir == "LeftB")
                animatorHandler.ChangeAnimationState("Walk_Backwards");
            else if (walkDir == "RightF")
                animatorHandler.ChangeAnimationState("Walk");
            else if (walkDir == "RightB") animatorHandler.ChangeAnimationState("Walk_Backwards");
        }
        else if (playerRb.velocity.magnitude <= 0.2f && !usingSkill && !isDashing)
        {
            animatorHandler.ChangeAnimationState("Idle");
        }
        else if (sprinting && !isDashing)
        {
            if (walkDir == "LeftF")
                animatorHandler.ChangeAnimationState("Run");
            else if (walkDir == "LeftB")
                animatorHandler.ChangeAnimationState("Run_Backwards");
            else if (walkDir == "RightF")
                animatorHandler.ChangeAnimationState("Run");
            else if (walkDir == "RightB") animatorHandler.ChangeAnimationState("Run_Backwards");
        }
        else if (isDashing)
        {
            if (dashDir == "LeftF")
                animatorHandler.ChangeAnimationState("Roll");

            else if (dashDir == "RightF")
                animatorHandler.ChangeAnimationState("Roll");

            else if (dashDir == "Roll") animatorHandler.ChangeAnimationState("Roll");
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
            dashDir = "LeftF";
        else if (movement.normalized.x > 0 && _cursorController.Direction == "Left")
            dashDir = "LeftB";
        else if (movement.normalized.x > 0 && _cursorController.Direction == "Right")
            dashDir = "RightF";
        else if (movement.normalized.x < 0 && _cursorController.Direction == "Right")
            dashDir = "RightB";
        else if (movement.normalized.x == 0) walkDir = "Roll";
    }

    private void WalkDirection()
    {
        if (movement.normalized.x < 0 && _cursorController.Direction == "Left")
            walkDir = "LeftF";
        else if (movement.normalized.x > 0 && _cursorController.Direction == "Left")
            walkDir = "LeftB";
        else if (movement.normalized.x > 0 && _cursorController.Direction == "Right")
            walkDir = "RightF";
        else if (movement.normalized.x < 0 && _cursorController.Direction == "Right")
            walkDir = "RightB";
        else if (movement.normalized.x == 0) walkDir = "RightF";
    }


    private IEnumerator Sprint()
    {
        while (true)
        {
            if (sprinting && stamina > 0)
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

            if (!canSprint && stamina > 30) canSprint = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void StunTrigger()
    {
        isStunned = true;
    }

    private IEnumerator Dash()
    {
        var duration = 1.15f;
        var elapsed = 0.0f;
        float stunTime;
        var actualDashPower = _dashpower;
        RollDirection();
        actualDashPower = _dashpower;
        stunTime = 0.1f;

        Vector2 direction = movement.normalized;
        yield return new WaitForSeconds(0.01f);
        canSprint = false;
        canDash = false;
        isDashing = true;
        stamina -= 25;

        Invoke("StunTrigger", duration);
        while (elapsed < duration && actualVelocity.magnitude > 0)
        {
            if (actualDashPower > 0)
            {
                actualDashPower -= 0.01f;
            }

            playerRb.AddForce(direction * actualDashPower, ForceMode2D.Impulse);
            elapsed += Time.deltaTime;
            yield return null;
        }
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
                footstepInterval = 0.30f;
            else
                footstepInterval = 0.46f;
            var x = Random.Range(1, 5);

            if (elapsed < footstepInterval)
            {
                elapsed += Time.deltaTime;
            }
            else
            {
                elapsed = 0;
                if (x == 1)
                    source.clip = walk1;
                else if (x == 2)
                    source.clip = walk2;
                else if (x == 3)
                    source.clip = walk2;
                else
                    source.clip = walk4;

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
        var played = false;
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