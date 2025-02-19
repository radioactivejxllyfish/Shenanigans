using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public GameObject enemy;
    public List<GameObject> enemies;
    public string currentAttack;
    public bool canAttack = true;
    public string currentState;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerRB");
        polygonCollider = GetComponentInChildren<PolygonCollider2D>();
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        cursorController = cursor.GetComponent<CursorController>();
        playerController = player.GetComponent<PlayerController>();

        
        StartCoroutine("Attack");
    }

    void Update()
    {
        SwordWield();
        
    }


    private void SwordWield()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position , 0.1f);
        if (cursorController.Direction == "Right")
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }
        else if (cursorController.Direction == "Left")
        {
            transform.localScale = new Vector3(-0.75f, 0.75f, 0.75f);
        }
        
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0) && canAttack && currentAttack != "BA1" && currentAttack != "BA2" && currentAttack != "BA3")
            {
                
                Debug.Log("BA1");
                currentAttack = "BA1";
                canAttack = false;
                ChangeAnimationState("BA1");
                CauseDMG(15f);
                yield return new WaitForSeconds(0.35f);
                canAttack = true;
            }
            else if (Input.GetMouseButtonDown(0) && canAttack && currentAttack == "BA1" && currentAttack != "BA2" && currentAttack != "BA3")
            {
                Debug.Log("BA2");
                currentAttack = "BA2";
                canAttack = false;
                ChangeAnimationState("BA2");
                CauseDMG(18f);
                yield return new WaitForSeconds(0.4f);
                canAttack = true;
            }
            else if (Input.GetMouseButtonDown(0) && canAttack && currentAttack == "BA2" && currentAttack != "BA3" && currentAttack != "BA1")
            {
                Debug.Log("BA3");
                currentAttack = "BA3";
                canAttack = false;
                ChangeAnimationState("BA3");
                CauseDMG(18f);
                yield return new WaitForSeconds(0.55f);
                canAttack = true;
            }
            else if (Input.GetMouseButtonDown(0) && canAttack && currentAttack == "BA3" && currentAttack != "BA2" && currentAttack != "BA1" && currentAttack != "BA4")
            {
                Debug.Log("BA4");
                currentAttack = "BA4";
                canAttack = false;
                ChangeAnimationState("BA4");
                CauseDMG(30f);
                yield return new WaitForSeconds(1.1f);
                canAttack = true;
            }
            else if (Input.GetKeyDown(KeyCode.E) && canAttack && playerController.stamina >= 45)
            {
                playerController.stamina -= 45;
                Instantiate(ShadowStrike);
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
                _enemy.GetComponent<BasicEnemy>().TakeDamage(dmg);
            }
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy inside");
                enemies.Add(collider.gameObject);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy outside");
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
