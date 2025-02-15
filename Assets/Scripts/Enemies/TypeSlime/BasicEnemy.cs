using System;
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

}