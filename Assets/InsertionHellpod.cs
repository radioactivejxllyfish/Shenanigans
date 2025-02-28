using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionHellpod : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public SpriteRenderer spriteRenderer;
    public GameObject player;
    public Animator animator;
    public ParticleSystem particleSystem0;
    public AudioSource audioSource;
    public CircleCollider2D circleCollider;

    public Sprite landed;
    public Vector3 landingSpot;
    public Vector3 velocity;
    public float speed;
    public float aoeDamage;

    public bool hasLanded;
    
    
    public void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        particleSystem0 = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();

        speed = 12f;
        aoeDamage = 300f;
        StartCoroutine(Launch());
    }

    void Update()
    {
        Transformer();
    }


    private void Transformer()
    {
        float height = Vector2.Distance(landingSpot, transform.position);
        velocity = (landingSpot - transform.position).normalized * speed;
        transform.eulerAngles = (transform.position - landingSpot).normalized;
        if (height > 3f)
        {
            rigidBody2D.velocity = velocity;

        }
        else if (height <= 2f)
        {
            rigidBody2D.velocity = Vector2.zero;
            hasLanded = true;
            StartCoroutine(DeployPlayer());
        }

    }


    private IEnumerator Launch()
    {
        yield return new WaitForSeconds(2.5f);
        animator.Play("Deploy");
    }

    private IEnumerator DeployPlayer()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(player, transform.position, transform.rotation);
        spriteRenderer.sprite = landed;
        spriteRenderer.sortingLayerID = -6;
    }
    
}
