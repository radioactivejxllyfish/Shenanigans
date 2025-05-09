using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caliber9mm : MonoBehaviour
{
    private CircleCollider2D _circleCollider2D;
    private Rigidbody2D _rigidbody2D;
    private float _bulletSpeed = 30f;
    private float _damage = 30f;
    private Vector3 dir;
    private GameObject enemy;
    private GameObject player;

    
    public GameObject target;
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Cursor");
        player = GameObject.FindGameObjectWithTag("PlayerRB");
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        dir = (target.transform.position - player.transform.position).normalized;
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        Transformer();
    }

    private void Transformer()
    {
        _rigidbody2D.velocity = dir * _bulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
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
