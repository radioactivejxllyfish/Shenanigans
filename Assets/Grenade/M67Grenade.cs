using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class M67Grenade : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public CircleCollider2D circleCollider2D;
    public CameraSmoother cameraSmoother;
    public ParticleSystem particleSystem0;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    
    public Ray2D ray;
    public float range;
    public float fuse;
    public float damage;
    public float travelTime;
    public float speed;
    public float distance;
    public float timeTravelled;
    public float rotationSpeed = 5f;
    public float targetAngle;
    public float radium;

    public GameObject player;
    public GameObject cursor;
    public GameObject enemy;
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> vulnerableEnemies = new List<GameObject>();
    public Vector2 positionToEnemy;
    public Vector2 throwArc;

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRB");
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        circleCollider2D = GetComponentInChildren<CircleCollider2D>();
        cameraSmoother = Camera.main.GetComponent<CameraSmoother>();
        particleSystem0 = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        damage = 120f;
        fuse = 3f;
        travelTime = 2.4f;
        range = 4f;
        speed = 4.5f;
        

        positionToEnemy = (cursor.transform.position - transform.position).normalized;
        Invoke("Explode", fuse);
        Invoke("DestroySprite", fuse);
        Destroy(gameObject, fuse + 4f);
        radium = Vector2.Distance(transform.position, cursor.transform.position);

    }

    void Update()
    {
        ThrowArc();
        
    }

    void FixedUpdate()
    {
        spriteRenderer.transform.Rotate(new Vector3(0, 0, Random.Range(-1,1)));
    }

    private void DestroySprite()
    {
        spriteRenderer.enabled = false;
    }
    private void ThrowArc()
    {

        float radius = Vector2.Distance(transform.position, player.transform.position);
        if (radium > radius)
        {
            rigidBody2D.velocity = positionToEnemy * speed;
        }
        else if (radium <= radius)
        {
            rigidBody2D.velocity = positionToEnemy * Mathf.Lerp(speed, 0, 2f);
        }
    }

    private void Explode()
    {
        particleSystem0.Play();
        audioSource.Play();
        cameraSmoother.CameraShake(0.25f, 0.08f);
        foreach (GameObject enemy in vulnerableEnemies)
        {
            enemy.GetComponent<BasicEnemy>().TakeDamage(damage);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider !=null)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                enemy = collider.gameObject;
                enemies.Add(enemy);
                if (Physics2D.Raycast(transform.position, (enemy.transform.position - transform.position).normalized, range))
                {
                    vulnerableEnemies.Add(enemy);
                    Gizmos.DrawRay(transform.position, (enemy.transform.position - transform.position).normalized);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
            vulnerableEnemies.Remove(enemy);
        }
    }
    
    
}
