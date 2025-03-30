using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCleanser : BasicEnemy
{

    private bool hasFoundCamera = false;
    private bool _hasLineOfSight = false;
    private bool _isRoaming = false;
    private bool _isDead = false;
    private CameraSmoother _cameraSmoother;
    private bool _attacking = false;
    


    List<GameObject> enemies = new List<GameObject>();
    public GameObject humanoidRootPart;

    void Start()
    {

        isStunned = false;
        isAlive = true;
        maxhealth = 150f;
        health = 150f;
        speed = 4f;
        damage = 15f;
        
        
        
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sight = GetComponentInChildren<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        StartCoroutine("SlamMove");
    }

    void Update()
    {
        LOSCheck();
        Movement();
        Mathf.Clamp(health, 0f, maxhealth);
        DeathHandler();
    }


    private void DeathHandler()
    {
        if (health <= 0f)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    private void Movement()
    {
        if (!_attacking)
        {
            if (rigidBody.velocity.x > 0)
            {
                humanoidRootPart.transform.localScale = new Vector3(3, 3, 3);
            }
            else if (rigidBody.velocity.x < 0)
            {
                humanoidRootPart.transform.localScale = new Vector3(-3, 3, 3);
            }

        
            float chance = Random.Range(0f, 1f);
            if (player != null && _hasLineOfSight)
            {
                _isRoaming = false;
                direction = (player.transform.position - transform.position).normalized;
                rigidBody.velocity = direction * speed;
            }
            else if (player == null && chance > 0.0125f && _isRoaming == false && !_hasLineOfSight)
            {
                direction = Vector2.zero;
                rigidBody.velocity = Vector2.zero;
            }
            else if (chance <= 0.0125f && _isRoaming == false && !_hasLineOfSight)
            {
                StartCoroutine(Roaming());
            }

        }

    }


    private void LOSCheck()
    {
        if (player != null)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position,(player.transform.position - transform.position).normalized);
            if (ray.collider != null)
            {
                if (ray.collider.CompareTag("PlayerRB"))
                {
                    Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized);
                    _hasLineOfSight = true;
                }
                else
                {
                    _hasLineOfSight = false;
                }
            }
        }
    }




    private IEnumerator Roaming()
    {
        yield return new WaitForSeconds(1f);
        float elapsed = 0.0f;
        float random = Random.Range(0f, 360f);
        Vector2 heading = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
        RaycastHit2D ray = Physics2D.Raycast(transform.position, heading);
        if (ray.collider != null)
        {
            if (!ray.collider.CompareTag("PlayerRB"))
            {
                heading = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
            }
        }
        _isRoaming = true;
        float duration = Random.Range(0.3f, 3f);
        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (ray.collider != null)
            {
                if (!ray.collider.CompareTag("PlayerRB"))
                {
                    heading = new Vector2(-heading.x, -heading.y);
                }
            }
            rigidBody.velocity = heading * 2f;

        }
        yield return new WaitForSeconds(Random.Range(2f, 6f));
        _isRoaming = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("PlayerRB"))
            {
                player = other.gameObject;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.gameObject.CompareTag("PlayerRB"))
            {
                player = null;
            }
        }
    }

    private IEnumerator SlamMove()
    {
        while (true)
        {
            if (player != null && Vector2.Distance(player.transform.position, transform.position)  < 2.5f &&!_isDead)
            {
                _attacking = true;
                direction = Vector2.zero;
                rigidBody.velocity = Vector2.zero;
                yield return new WaitForSeconds(0.5f);
                if (Vector2.Distance(player.transform.position, transform.position) < 2.5f)
                {             
                    rigidBody.velocity = Vector2.zero;
                    Debug.Log("SlamAttack");
                    yield return new WaitForSeconds(0.8f);
                    player.GetComponent<PlayerController>().isStunned = true;
                    player.GetComponent<PlayerVarPool>().TakeDamage(70);
                    player.GetComponent<PlayerVarPool>().ApplyKnockback(transform.position, 5f);
                    yield return new WaitForSeconds(0.4f);
                    player.GetComponent<PlayerController>().isStunned = false;
                    _attacking = false;

                }
                else
                {
                    _attacking = false;
                    yield return null;
                }
            
                yield return null;
            }
            yield return null;

        }
    }


}