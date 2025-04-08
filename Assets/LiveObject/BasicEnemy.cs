using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

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
    
    
    
    public void TakeDamage(float dmg)
    {
        health -= damage;
        speed -= speed * (3f/100f) ;
        StartCoroutine(StunPeek());
    }

    public IEnumerator StunPeek()
    {
        rigidBody.transform.rotation = Quaternion.Euler(0,0, Random.Range(-5f, 5f));
        yield return new WaitForSeconds(0.05f);
        rigidBody.transform.rotation = Quaternion.Euler(0,0, 0);
        yield return null;
    }
    

}