using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCleanser : BasicEnemy
{

 
    private bool _hasLineOfSight = false;
    private bool _isRoaming = false;
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


}