using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RiotShield : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public PolygonCollider2D polygonCollider;
    public GameObject player;
    public GameObject cursor;
    public Animator animator;
    public CursorController cursorController;
    public PlayerController playerController;
    public CameraSmoother cameraSmoother;
    public List<GameObject> enemies;
    public AudioSource audioFX;
    public Vector3 direction;
    
    public float damage;
    public bool canAttack = true;
    private string currentState;
    private bool blocking = false;
    private bool canBlock = true;
    private void OnEnable()
    {
        StartCoroutine(Init());
        if (playerController != null)
        {
            StartCoroutine("BasicAttack");
        }
        else
        {
            Debug.Log(playerController);
        }
    }

    void FixedUpdate()
    {
        Mathf.RoundToInt(direction.x);
        direction = (cursor.transform.position - transform.position).normalized;
        transform.position = player.transform.position;
        transform.right = Vector3.Lerp(transform.right, direction.normalized, 0.1f);
        
        
        if (cursorController.Direction == "Right" && !playerController.isDashing)
        {
            
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (cursorController.Direction == "Left" && !playerController.isDashing)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }

        if (transform.rotation == Quaternion.Euler(0,180,0))
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }





    private IEnumerator BasicAttack()
    {
        while (true)
        {
            if (Input.GetMouseButton(0) && !playerController.isDead && canAttack)
            {
                int x = Random.Range(1, 4);
                canBlock = false;
                canAttack = false;
                CauseDMG(damage);
                switch (x)
                {
                    case 1: ChangeAnimationState("Strike");
                        break;
                    case 2: ChangeAnimationState("Strike2");
                        break;
                    case 3: ChangeAnimationState("Strike3");
                        break;
                }
                yield return new WaitForSeconds(0.8f);
                canAttack = true;
                canBlock = true;

            }
            else if (Input.GetMouseButton(1) && !playerController.isDead && canAttack && canBlock)
            {
                Debug.Log("Blocking");
                blocking = true;
                ChangeAnimationState("Block");
                playerController.currentSpeed = playerController.slowSpd;
                canAttack = false;
                yield return null;
                blocking = false;
                canAttack = true;
            }
            else if (canAttack && !blocking && canBlock)
            {
                canBlock = true;
                canAttack = true;
                playerController.currentSpeed = playerController.speed;
                ChangeAnimationState("Idle");
                yield return null;
            }
            yield return null;
        }
    }
    
    
    
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState); 
        currentState = newState;
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
    
    private IEnumerator Init()
    {        
        cameraSmoother = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraSmoother>();
        player = GameObject.FindGameObjectWithTag("PlayerRB");
        polygonCollider = GetComponentInChildren<PolygonCollider2D>();
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        cursorController = cursor.GetComponent<CursorController>();
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        damage = 70f;
        yield return new WaitForSeconds(0.5f);

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
}
