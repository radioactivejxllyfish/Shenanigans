using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeExplosive : BasicEnemy
{

    private bool hasFoundCamera = false;
    private bool _hasLineOfSight = false;
    private bool _isRoaming = false;
    private bool _exploding = false;
    private bool _isDead = false;
    private CameraSmoother _cameraSmoother;



    [SerializeField] private Sprite _roaming;
    [SerializeField] private Sprite _chasing;
    [SerializeField] private Sprite _exploding1;
    [SerializeField] private Sprite _exploding2;
    [SerializeField] private Sprite _exploded;


    public GameObject bloodSplatter;
    public ParticleSystem explosiveParticles;

    
    
    void Start()
    {

        isStunned = false;
        isAlive = true;
        maxhealth = 150f;
        health = 150f;
        speed = 4f;
        damage = 120f;

    
        _cameraSmoother = Camera.main.GetComponent<CameraSmoother>();
        if (_cameraSmoother == null)
        {
            hasFoundCamera = false;
        }
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sight = GetComponentInChildren<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        StartCoroutine(Explosive());
    }

    void Update()
    {
        if (!hasFoundCamera)
        {
            _cameraSmoother = Camera.main.GetComponent<CameraSmoother>();
            if (_cameraSmoother != null)
            {
                hasFoundCamera = true;
            }
        }
        LOSCheck();
        Movement();
        Mathf.Clamp(health, 0f, maxhealth);
        DeathHandler();
    }


    private void DeathHandler()
    {
        if (health <= 0f)
        {
            rigidBody.velocity = Vector2.zero;
            _isDead = true;
            Destroy(gameObject, 7f);
        }
    }

    private void Movement()
    {
        if (rigidBody.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rigidBody.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            
        }
        float chance = Random.Range(0f, 1f);
        if (player != null && _hasLineOfSight && !_exploding && !_isDead)
        {
            spriteRenderer.sprite = _chasing;

            _isRoaming = false;
            direction = (player.transform.position - transform.position).normalized;
            rigidBody.velocity = direction * speed;
        }
        else if (player == null && chance > 0.0125f && _isRoaming == false && !_hasLineOfSight && !_exploding && !_isDead)
        {
            spriteRenderer.sprite = _roaming;

            direction = Vector2.zero;
            rigidBody.velocity = Vector2.zero;
        }
        else if (chance <= 0.0125f && _isRoaming == false && !_hasLineOfSight && !_exploding && !_isDead)
        {
            spriteRenderer.sprite = _roaming;

            StartCoroutine(Roaming());
            Debug.Log("Roam");
        }
        
    }


    private void LOSCheck()
    {
        if (player != null)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position,(player.transform.position - transform.position).normalized);
            if (ray.collider != null)
                {
                    if (ray.collider.CompareTag("PlayerRB"))
                    {
                        Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized);
                        _hasLineOfSight = true;
                    }
                    else
                    {
                        _hasLineOfSight = false;
                    }
                }
        }
    }



    private IEnumerator Roaming()
    {
        float elapsed = 0.0f;
        float random = Random.Range(0f, 360f);
        Vector2 heading = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
        RaycastHit2D ray = Physics2D.Raycast(transform.position, heading);
        if (ray.collider != null)
        {
            if (!ray.collider.CompareTag("PlayerRB"))
            {
                heading = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
            }
        }
        _isRoaming = true;
        float duration = Random.Range(0.3f, 3f);
        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (ray.collider != null)
            {
                if (!ray.collider.CompareTag("PlayerRB"))
                {
                    heading = new Vector2(-heading.x, -heading.y);
                }
            }
            rigidBody.velocity = heading * 2f;

        }
        Debug.Log("Roaming");
        yield return new WaitForSeconds(Random.Range(2f, 6f));
        _isRoaming = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("PlayerRB"))
            {
                player = other.gameObject;
                Debug.Log("Player Entered");
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.gameObject.CompareTag("PlayerRB"))
            {
                player = null;
                Debug.Log("Player Exited");
            }
        }
    }

    private IEnumerator Explosive()
    {
        while (true)
        {
            if (player != null && Vector2.Distance(player.transform.position, transform.position)  < 2.5f &&!_isDead)
            {
                spriteRenderer.sprite = _chasing;
                _exploding = true;
                direction = Vector2.zero;
                rigidBody.velocity = Vector2.zero;
                yield return new WaitForSeconds(0.5f);
                if (Vector2.Distance(player.transform.position, transform.position) < 2.5f)
                {                    
                    rigidBody.velocity = Vector2.zero;
                    yield return new WaitForSeconds(0.08f);
                    spriteRenderer.sprite = _exploding1;
                    yield return new WaitForSeconds(0.08f);
                    spriteRenderer.sprite = _exploding2;
                    yield return new WaitForSeconds(0.08f);
                    spriteRenderer.sprite = _exploding1;
                    yield return new WaitForSeconds(0.08f);
                    spriteRenderer.sprite = _exploding2;
                    yield return new WaitForSeconds(0.08f);
                    spriteRenderer.sprite = _exploding1;
                    yield return new WaitForSeconds(0.08f);
                    spriteRenderer.sprite = _exploding2;
                    yield return new WaitForSeconds(0.3f);

                    explosiveParticles.Play();
                    if (Vector2.Distance(player.transform.position, transform.position) < 3f)
                    {
                        player.GetComponent<PlayerVarPool>().TakeDamage(damage);
                        _cameraSmoother.CameraShake(0.2f, 0.25f);
                        spriteRenderer.sprite = _exploded;
                        health = 0f;
                        Destroy(gameObject, 7f);
                        rigidBody.velocity = Vector2.zero;
                        yield return new WaitForSeconds(99f);
                    }
                    else
                    {
                        _cameraSmoother.CameraShake(0.15f, 0.08f);
                        spriteRenderer.sprite = _exploded;
                        health = 0f;
                        Destroy(gameObject, 7f);
                        rigidBody.velocity = Vector2.zero;
                        yield return new WaitForSeconds(99f);
                    }

                }
                else
                {
                    spriteRenderer.sprite = _roaming;
                    _exploding = false;
                    yield return null;
                }

                yield return null;

            }
            yield return null;

        }


    }
}
