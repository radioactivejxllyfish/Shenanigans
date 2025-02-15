using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRifleBullet : GenericRifleBehaviour
{
    private float velocity = 35f;
    private Vector2 target;
    private Rigidbody2D rb;
    private CircleCollider2D boulette;
    private GameObject _cursor;
    private GameObject _player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boulette = GetComponent<CircleCollider2D>();
        _cursor = GameObject.FindGameObjectWithTag("Cursor");
        _player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        target = (_cursor.transform.position - _player.transform.position).normalized;
        damage = 35f;
    }

    void Update()
    {
        rb.velocity = target * velocity;
        Destroy(gameObject, 2f);
    }

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<GruntEnemyBehaviour>().TakeDamage(damage);
                other.GetComponent<Rigidbody2D>().AddForce(-(_player.transform.position - other.transform.position).normalized * velocity);
            }
        }
    }
    */
}
