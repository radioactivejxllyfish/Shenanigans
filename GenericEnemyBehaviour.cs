using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenericEnemyBehaviour : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    private float _speed;
    private float _health;
    private CircleCollider2D _detector;

    private void Start()
    {
        _speed = 2.6f;
        _health = 100f;
        _rigidbody = GetComponent<Rigidbody2D>();
        _detector = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (_player != null)
        {
            Vector2 direction = _player.transform.position - transform.position;
            _rigidbody.velocity = direction.normalized * _speed;
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (collider.CompareTag("PlayerHitbox"))
            {
                Debug.Log("Player Hit");
                _player = collider.gameObject;
            }
        }
    }
}
