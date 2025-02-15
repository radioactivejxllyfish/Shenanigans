using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeSlime : BasicEnemy
{
    void Start()
    {
        isStunned = false;
        isAlive = true;
        maxhealth = 80f;
        health = 80f;
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
        Movement();
        Mathf.Clamp(health, 0f, maxhealth);
    }



    private void Movement()
    {
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
            rigidBody.velocity = direction * speed;
        }
        else
        {
            direction = Vector2.zero;
            rigidBody.velocity = Vector2.zero;
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("PlayerRB"))
            {
                player = other.gameObject;
                Debug.Log("Player Entered");
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
                Debug.Log("Player Exited");
            }
        }
    }
}