using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caliber50AE : MonoBehaviour
{
    private CircleCollider2D _circleCollider2D;
    private Rigidbody2D _rigidbody2D;
    private float _bulletSpeed = 50f;
    private float _damage = 45f;
    private Vector3 dir;
    private GameObject enemy;
    public GameObject gun;
    public GameObject target;
    
    void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        dir = (target.transform.position - transform.position).normalized;
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        Transformer();
    }

    private void Transformer()
    {
     
        transform.right = dir;
        _rigidbody2D.velocity = dir * _bulletSpeed;

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Hit");
                enemy = other.gameObject;
                enemy.GetComponent<BasicEnemy>().TakeDamage(_damage);
                Destroy(gameObject);
            }
            else if (!other.gameObject.CompareTag("Objective") && !other.gameObject.CompareTag("PlayerRB") && !other.gameObject.CompareTag("Insertion") && !other.gameObject.CompareTag("Loot") && !!other.gameObject.CompareTag("Sight"))
            {
                Destroy(gameObject);
            }
        }
    }
}