using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CombatMode : MonoBehaviour
{
    public GameObject player;
    private CircleCollider2D _circleCollider2D;
    private float damage = 60f;
    private float fireRate = 0.15f;
    private GameObject target;
    private List<GameObject> enemies = new List<GameObject>();
    private float closestDistance = Mathf.Infinity;
    GameObject closestEnemy = null;
    public PlayerAnimationHandler playerAnimationHandler;
    public GameObject bullet;
    
    private int magazine = 120;
    private int ammo = 120;

    private float elapsed = 0;


    void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Update()
    {

        TargetSelect();
        if (target != null)
        {
            if (!target.gameObject.GetComponent<BasicEnemy>().isAlive)
            {
                target = null;
            }

            if (playerAnimationHandler.Direction == "Right")
            {
                transform.right = target.transform.position - transform.position;
            }
            else if (playerAnimationHandler.Direction == "Left")
            {
                transform.right = -(target.transform.position - transform.position);
            }
            if (ammo > 0)
            {
                if (elapsed < fireRate)
                {
                    elapsed += Time.deltaTime;
                }
                else
                {
                    StartCoroutine("Shoot");

                    elapsed = 0;
                }
            }
        }
        else
        {
            transform.right = Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine("Shoot");
        }
    }

    void TargetSelect()
    {
        if (enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy == null) continue;
                if (!enemy.gameObject.GetComponent<BasicEnemy>().isAlive) continue;
                float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    if (enemy.gameObject.GetComponent<BasicEnemy>().isAlive)
                    {
                        closestEnemy = enemy;
                    }
                }
                target = closestEnemy;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Enemy") && target == null && other.gameObject.GetComponent<BasicEnemy>().isAlive)
            {
                enemies.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                if (collider.gameObject == target)
                {
                    target = null;
                }
                enemies.Remove(collider.gameObject);
            }
        }
    }

    private IEnumerator Shoot()
    {
        if (playerAnimationHandler.Direction == "Right")
        {
            Instantiate(bullet, transform.position, transform.rotation).GetComponent<Caliber50AE>().target = target;
        }
        else if (playerAnimationHandler.Direction == "Left")
        {
            Instantiate(bullet, transform.position, transform.rotation).GetComponent<Caliber50AE>().target = target;
        }
        ammo -= 1;
        yield return null;

    }
}
