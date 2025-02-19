using System;
using System.Collections;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float health;
    public float maxhealth;
    public float speed;
    public float damage;

    public bool isAlive;
    public bool isStunned;
    
    public Vector2 direction;

    public Animator animator;
    public GameObject player;
    public Rigidbody2D rigidBody;
    public CircleCollider2D sight;
    public SpriteRenderer spriteRenderer;

    
    

    public void TakeDamage(float damage)
    {
        health -= damage;
        speed -= speed * (5f/100f) ;
    }

    public IEnumerator knockBackCR(float strength, Vector2 kbdir)
    {
        float duration = 0.5f;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rigidBody.gravityScale = 1;
            rigidBody.AddForce((kbdir.normalized + Vector2.up) * strength, ForceMode2D.Force);
            yield return null;
        }

        yield return null;


    }

}