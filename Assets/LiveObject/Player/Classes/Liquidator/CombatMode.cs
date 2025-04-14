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
    private float damage = 45f;
    private float fireRate = 0.12f;
    private GameObject target;
    private List<GameObject> enemies = new List<GameObject>();
    private float closestDistance = Mathf.Infinity;
    GameObject closestEnemy = null;
    public PlayerAnimationHandler playerAnimationHandler;
    public GameObject bullet;
    
    private int magazine = 120;
    private int ammo = 120;

    private bool isResettingOrientation = false;
    private float elapsed = 0;

    public bool combatMode = false;


    void Start()
    {
        gameObject.transform.DORotate(Vector2.right, 1f);

        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (combatMode)
        {
            if (target != null)
            {
                var enemy = target.GetComponent<BasicEnemy>();

                if (enemy == null)
                {
                    target = null;
                }
                else if (enemy.health < 0)
                {
                    target = null;
                }
                else if (Vector3.Distance(player.transform.position, target.transform.position) > 15f)
                {
                    target = null;
                }
            }
            TargetSelect();
            FireAtTarget();
        }

    }


    void TargetSelect()
    {
        if (target == null)
        {
            closestDistance = Mathf.Infinity;
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(enemy.transform.position,transform.position);
                if (distance < closestDistance && distance < 15f && enemy.gameObject.GetComponent<BasicEnemy>().health > 0)
                {
                    target = enemy.gameObject;
                }
                else continue;
            }
        }
        else if (target != null)
        {
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(enemy.transform.position,transform.position);
                if (distance < Vector2.Distance(target.transform.position,transform.position) && distance < 15f && enemy.gameObject.GetComponent<BasicEnemy>().health > 0)
                {
                    target = enemy.gameObject;
                }
                else continue;
            }
        }

    }

    private void FireAtTarget()
    {
        
        if (target != null)
        {
            gameObject.transform.DOKill(false);
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
            if (!isResettingOrientation)
            {
                StartCoroutine("ResetOrientation");
                isResettingOrientation = true;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Enemy") && other.gameObject.GetComponent<BasicEnemy>().isAlive)
            {
                if (!enemies.Contains(other.gameObject))
                    enemies.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            enemies.Remove(collider.gameObject);
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

    private IEnumerator ResetOrientation()
    {
        gameObject.transform.DORotate(Vector2.right, 1);
        yield return null;
        isResettingOrientation = false;
    }
}
