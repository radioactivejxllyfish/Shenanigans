using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyTypeExplosive : BasicEnemy
{
    private bool hasFoundCamera;
    private bool _hasLineOfSight;
    private bool _isRoaming;
    private bool _exploding;
    private bool _isDead;
    private CameraSmoother _cameraSmoother;
    public GameObject Renderer;


    private readonly List<GameObject> enemies = new();

    public ParticleSystem explosiveParticles;


    private void Start()
    {
        path = GetComponent<AIPath>();
        isStunned = false;
        isAlive = true;
        maxhealth = 450f;
        health = 450f;
        speed = 4f;
        damage = 120f;
        moveSpeed = speed;


        _cameraSmoother = Camera.main.GetComponent<CameraSmoother>();
        if (_cameraSmoother == null) hasFoundCamera = false;

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sight = GetComponentInChildren<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(Explosive());
    }

    private void Update()
    {
        if (!hasFoundCamera)
        {
            _cameraSmoother = Camera.main.GetComponent<CameraSmoother>();
            if (_cameraSmoother != null) hasFoundCamera = true;
        }

        LOSCheck();
        if (!_isDead)
        {
            Movement();
            if (player != null)
                if (_hasLineOfSight && !_exploding)
                {
                    path.canMove = true;
                    path.maxSpeed = moveSpeed;
                    path.destination = player.transform.position;
                }
        }
        else
        {
            path.canMove = false;
            path.maxSpeed = 0f;
            path.destination = transform.position;
        }

        Mathf.Clamp(health, 0f, maxhealth);
        DeathHandler();
    }


    private void DeathHandler()
    {
        if (health <= 0f)
        {
            if (!_exploding && health <= 0)
            {
            }

            rigidBody.velocity = Vector2.zero;
            _isDead = true;
            Destroy(gameObject, 7f);
        }
    }

    private void Movement()
    {

        if (path.velocity.x > 0)
        {
            Debug.Log("Moving right");
            Renderer.transform.localScale = new Vector3(-4, 4, 4);
        }
        else if (path.velocity.x <= 0)
        {
            Debug.Log("Moving left");

            Renderer.transform.localScale = new Vector3(4, 4, 4);
        }
        

        var chance = Random.Range(0f, 1f);
        if (player != null && _hasLineOfSight && !_exploding)
        {
            _isRoaming = false;
            direction = (player.transform.position - transform.position).normalized;
            rigidBody.velocity = direction * speed;
        }
        else if (player == null && chance > 0.0125f && _isRoaming == false && !_hasLineOfSight && !_exploding)
        {
            direction = Vector2.zero;
            rigidBody.velocity = Vector2.zero;
        }
        else if (chance <= 0.0125f && _isRoaming == false && !_hasLineOfSight && !_exploding)
        {
            StartCoroutine(Roaming());
        }
    }


    private void LOSCheck()
    {
        if (player != null)
        {
            var ray = Physics2D.Raycast(transform.position,
                (player.transform.position - transform.position).normalized);
            if (ray.collider != null)
            {
                if (ray.collider.CompareTag("PlayerRB"))
                {
                    Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized);
                    _hasLineOfSight = true;
                }
                else
                {
                    StartCoroutine("ForgetLOS");
                }
            }
        }
    }

    private IEnumerator ForgetLOS()
    {
        yield return new WaitForSeconds(Random.Range(10f, 15f));
        _hasLineOfSight = false;
    }

    private IEnumerator Roaming()
    {
        _isRoaming = true;

        path.canMove = true;
        path.maxSpeed = moveSpeed * 0.45f;
        path.destination = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3));
        Debug.Log("Roaming started");

        var timeout = Random.Range(8f, 12f);
        var elapsed = 0f;

        while (!path.reachedDestination && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (path.reachedDestination)
            yield return new WaitForSeconds(Random.Range(5, 12));
        else
            path.destination = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));

        yield return new WaitForSeconds(Random.Range(5, 12));
        _isRoaming = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Enemy")) enemies.Add(other.gameObject);

            if (other.gameObject.CompareTag("PlayerRB")) player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.gameObject.CompareTag("Enemy")) enemies.Remove(collider.gameObject);

            if (collider.gameObject.CompareTag("PlayerRB")) player = null;
        }
    }

    private IEnumerator Explosive()
    {
        while (true)
        {
            if (player != null && Vector2.Distance(player.transform.position, transform.position) < 2.5f && !_isDead)
            {
                _exploding = true;
                direction = Vector2.zero;
                rigidBody.velocity = Vector2.zero;
                yield return new WaitForSeconds(0.5f);
                if (Vector2.Distance(player.transform.position, transform.position) < 2.5f)
                {
                    for (var i = 0; i < 10; i++) yield return new WaitForSeconds(0.08f);

                    rigidBody.velocity = Vector2.zero;

                    explosiveParticles.Play();
                    if (Vector2.Distance(player.transform.position, transform.position) < 3f)
                    {
                        foreach (var enemy in enemies)
                            if (Vector2.Distance(enemy.gameObject.transform.position, transform.position) < 3f)
                                if (Physics2D.Raycast(transform.position,
                                        (enemy.transform.position - transform.position).normalized, 3f))
                                    enemy.gameObject.GetComponent<BasicEnemy>().TakeDamage(damage);

                        if (Physics2D.Raycast(transform.position,
                                (player.transform.position - transform.position).normalized, 3f))
                            player.GetComponent<PlayerController>().TakeDamage(damage, "Explosive");

                        _cameraSmoother.CameraShake(0.2f, 0.25f);
                        health = 0f;
                        Destroy(gameObject, 7f);
                        rigidBody.velocity = Vector2.zero;
                        yield return new WaitForSeconds(99f);
                    }
                    else
                    {
                        foreach (var enemy in enemies)
                            if (Vector2.Distance(enemy.gameObject.transform.position, transform.position) < 3f)
                                if (Physics2D.Raycast(transform.position,
                                        (enemy.transform.position - transform.position).normalized, 3f))
                                    enemy.gameObject.GetComponent<BasicEnemy>().TakeDamage(damage);

                        _cameraSmoother.CameraShake(0.15f, 0.08f);
                        health = 0f;
                        Destroy(gameObject, 7f);
                        rigidBody.velocity = Vector2.zero;
                        yield return new WaitForSeconds(99f);
                    }
                }
                else
                {
                    _exploding = false;
                    yield return null;
                }

                yield return null;
            }

            yield return null;
        }
    }
}