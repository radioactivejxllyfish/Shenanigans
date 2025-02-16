using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public PolygonCollider2D polygonCollider;
    public GameObject player;
    public GameObject cursor;
    public Animator animator;


    public string currentAttack;
    public bool canAttack = true;
    public string currentState;
    
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        polygonCollider = GetComponentInChildren<PolygonCollider2D>();
        player = GameObject.FindGameObjectWithTag("PlayerRB");
        cursor = GameObject.FindGameObjectWithTag("Cursor");
        StartCoroutine("Attack");

    }

    void Update()
    {
        
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0) && canAttack && currentAttack != "BA1" && currentAttack != "BA2")
            {
                Debug.Log("BA1");
                currentAttack = "BA1";
                canAttack = false;
                ChangeAnimationState("BA1");
                yield return new WaitForSeconds(0.3f);
                canAttack = true;
            }
            else if (Input.GetMouseButtonDown(0) && canAttack && currentAttack == "BA1" && currentAttack != "BA2" && currentAttack != "BA3")
            {
                Debug.Log("BA2");
                currentAttack = "BA2";
                canAttack = false;
                ChangeAnimationState("BA2");
                yield return new WaitForSeconds(0.25f);
                canAttack = true;
            }
            else if (Input.GetMouseButtonDown(0) && canAttack && currentAttack == "BA2" && currentAttack != "BA3" && currentAttack != "BA1")
            {
                Debug.Log("BA3");
                currentAttack = "BA3";
                canAttack = false;
                ChangeAnimationState("BA3");
                yield return new WaitForSeconds(0.45f);
                canAttack = true;
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
}
