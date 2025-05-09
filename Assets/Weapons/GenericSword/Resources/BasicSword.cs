using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BasicSword : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public PolygonCollider2D polygonCollider;
    public GameObject player;
    public GameObject cursor;
    public Animator animator;
    public GameObject ShadowStrike;
    public CursorController cursorController;
    public PlayerController playerController;
    public CameraSmoother cameraSmoother;
    public GameObject enemy;
    public List<GameObject> enemies;
    public AudioSource audioFX;
    

    
    public AudioClip slashfx1;
    public AudioClip slashfx2;
    public AudioClip slashfx3;
    public AudioClip shadowfx;

    public float damage;

    public bool canAttack = true;
    public string currentAttack;
    public string currentState;
    
    void Start()
    {
        cameraSmoother = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraSmoother>();
        player = GameObject.FindGameObjectWithTag("PlayerRB");
        polygonCollider = GetComponentInChildren<PolygonCollider2D>();
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        cursorController = cursor.GetComponent<CursorController>();
        playerController = player.GetComponent<PlayerController>();
        StartCoroutine(Init());
        damage = 30f;
    }

    private void OnEnable()
    {
        StartCoroutine("Attack");
    }
    void FixedUpdate()
    {
        SwordWield();
        
    }


    private void SwordWield()
    {
        transform.position = player.transform.position;
        if (cursorController.Direction == "Right" && !playerController.isDashing)
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
        else if (cursorController.Direction == "Left" && !playerController.isDashing)
        {
            transform.localScale = new Vector3(-0.75f, 0.75f, 0.75f);
        }
        
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0) && canAttack && currentAttack != "BA1" && currentAttack != "BA2")
            {
                SlashFX();

                currentAttack = "BA1";
                canAttack = false;
                ChangeAnimationState("BA1");
                CauseDMG(damage /2);
                yield return new WaitForSeconds(0.35f);
                canAttack = true;
            }
            else if (Input.GetMouseButtonDown(0) && canAttack && currentAttack == "BA1" && currentAttack != "BA2" && currentAttack != "BA3")
            {
                SlashFX();

                currentAttack = "BA2";
                canAttack = false;
                ChangeAnimationState("BA2");
                CauseDMG(damage * (2/3));
                yield return new WaitForSeconds(0.4f);
                canAttack = true;
            }
            else if (Input.GetMouseButtonDown(0) && canAttack && currentAttack == "BA2" && currentAttack != "BA3" && currentAttack != "BA1")
            {
                SlashFX();

                currentAttack = "BA3";
                canAttack = false;
                ChangeAnimationState("BA3");
                CauseDMG(damage);
                yield return new WaitForSeconds(0.55f);
                canAttack = true;
            }
            else if (Input.GetKeyDown(KeyCode.E) && canAttack && playerController.stamina >= 45 && !playerController.usingSkill)
            {
                audioFX.clip = shadowfx;
                audioFX.Play();
                
                canAttack = false;
                playerController.usingSkill = true;
                playerController.stamina -= 45;
                Instantiate(ShadowStrike, transform.position, transform.rotation);
                if (cameraSmoother != null)
                {
                    cameraSmoother.CameraShake(0.1f, 5f);

                }
                yield return new WaitForSeconds(5f);
                playerController.usingSkill = false;
                canAttack = true; 
            }
            yield return null;
        }
        
    }
    
    private void CauseDMG(float dmg)
    {
        if (enemies != null)
        {
            foreach (GameObject _enemy in enemies)
            {
                if (_enemy != null)
                {
                    _enemy.GetComponent<BasicEnemy>().TakeDamage(dmg);

                }
            }
        }
    }

    private void SlashFX()
    {
        int rr = Random.Range(1, 3);
        if (rr == 1)
        {
            audioFX.clip = slashfx1;
            audioFX.Play();

        }
        else if (rr == 2)
        {
            audioFX.clip = slashfx2;
            audioFX.Play();

        }
        else if (rr == 3)
        {
            audioFX.clip = slashfx3;
            audioFX.Play();

        }
    }

    private IEnumerator Init()
    {        
        yield return new WaitForSeconds(0.5f);
        cameraSmoother = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraSmoother>();
    }
    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                enemies.Add(collider.gameObject);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
        }
            
    }

    
    
    

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState); 
        currentState = newState;
    }
}
