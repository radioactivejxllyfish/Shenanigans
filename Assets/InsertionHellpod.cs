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
    public bool hasDeployedPlayer = false;
    public bool hasLanded;
    public float height;


    private string currentState;
    
    
    public void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        particleSystem0 = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        circleCollider = GetComponent<CircleCollider2D>();
    
        speed = 25f;
        aoeDamage = 300f;
    }

    void Update()
    {
        Transformer();
    }


    private void Transformer()
    {
        height = Vector2.Distance(landingSpot, transform.position);
        velocity = (landingSpot - transform.position).normalized * speed;
        transform.up = -velocity.normalized;
        if (height > 3f)
        {
            ChangeAnimationState("Deploy");
            rigidBody2D.velocity = velocity;

        }
        else if (height <= 1f)
        {
            ChangeAnimationState("Landed");
            rigidBody2D.velocity = Vector2.zero;
            if (!hasDeployedPlayer)
            {
                StartCoroutine(DeployPlayer());
                hasDeployedPlayer = true;
            }
        }
    }
    

    private IEnumerator DeployPlayer()
    {
        spriteRenderer.sprite = landed;
        particleSystem0.Play();
        yield return new WaitForSeconds(1f);
        Instantiate(player, transform.position, Quaternion.Euler(0,0,0));
        hasLanded = true;
    }
    
    
    public void ChangeAnimationState(string newState)
    {
        // stop the animation from interrupting itself
        if (currentState == newState) return;
        // play animation
        animator.Play(newState); 
        // reassign current state   
        currentState = newState;
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject enemy;
        List<GameObject> enemies = new List<GameObject>();
        if (collider !=null)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                enemy = collider.gameObject;
                enemies.Add(enemy);
            }
        }

        if (!hasLanded && height <= 7f)
        {
            foreach (GameObject enemyObj in enemies)
            {
                enemyObj.gameObject.GetComponent<BasicEnemy>().TakeDamage(aoeDamage);
            }
        }

    }

}
