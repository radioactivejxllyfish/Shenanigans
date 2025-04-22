using System.Collections;
using DG.Tweening;
using Pathfinding;
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

    public AIPath path;
    public float moveSpeed;
    public Transform target;


    public void TakeDamage(float dmg)
    {
        health -= damage;
        speed -= speed * (3f / 100f);
        // StartCoroutine(StunPeek());
    }

    public IEnumerator StunPeek()
    {
        gameObject.transform.DOShakeRotation(1f);
        gameObject.transform.DOKill();
        yield return null;
    }
}