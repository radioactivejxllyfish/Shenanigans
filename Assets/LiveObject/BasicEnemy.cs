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
    public ParticleSystem bloodSplatter;
    
    

    public void TakeDamage(float damage)
    {
        StartCoroutine(BloodSplatter());
        health -= damage;
        speed -= speed * (15f/100f) ;
        StartCoroutine(StunPeek());
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

    public IEnumerator StunPeek()
    {
        rigidBody.transform.rotation = Quaternion.Euler(0,0, Random.Range(-20f, 20f));
        yield return new WaitForSeconds(0.25f);
        rigidBody.transform.rotation = Quaternion.Euler(0,0, 0);
        yield return null;
    }

    public IEnumerator BloodSplatter()
    {
        Instantiate(bloodSplatter, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.4f);
    }

}