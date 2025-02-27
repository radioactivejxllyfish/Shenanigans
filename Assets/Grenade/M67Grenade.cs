using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class M67Grenade : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public CircleCollider2D circleCollider2D;
    public CameraSmoother cameraSmoother;
    public ParticleSystem particleSystem0;
    public SpriteRenderer spriteRenderer;
    
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
    
    public GameObject cursor;
    public GameObject enemy;
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> vulnerableEnemies = new List<GameObject>();
    public Vector2 positionToEnemy;
    public Vector2 throwArc;

    
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        circleCollider2D = GetComponentInChildren<CircleCollider2D>();
        cameraSmoother = Camera.main.GetComponent<CameraSmoother>();
        particleSystem0 = GetComponentInChildren<ParticleSystem>();
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        damage = 120f;
        fuse = 3f;
        travelTime = 2.4f;
        range = 3.5f;
        speed = 4.5f;
        

        positionToEnemy = (cursor.transform.position - transform.position).normalized;
        Invoke("Explode", fuse);
        Invoke("DestroySprite", fuse);
        Destroy(gameObject, fuse + 0.5f);

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
        float stopDistance = 0.25f; // Distance at which it should slow down
        float deceleration = 0.54f; // Adjust to control the smooth stop
        float distance = Vector2.Distance(transform.position, positionToEnemy);

        // Calculate remaining distance

        if (distance > stopDistance)
        {
            speed = Mathf.Lerp(speed, 0, Time.deltaTime * deceleration);

            rigidBody2D.velocity = positionToEnemy * speed;
        }
        else if (distance < stopDistance)
        {
            rigidBody2D.velocity = Vector2.zero;
        }
    }

    private void Explode()
    {
        particleSystem0.Play();
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
