using System;
using UnityEngine;

public class GenericEnemyBehaviour : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    private float _speed;
    private float _health;
    private Vector2 direction;
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
            direction = _player.transform.position - transform.position;
            _rigidbody.velocity = direction.normalized * _speed;
        }

        if (_health <= 50f)
        {
            _speed = 2.0f;
        }
        if (_health <= 0f)
        {
            Destroy(gameObject);
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHitbox"))
        {
            _player = null;
            Debug.Log("player Exit");
            direction = Vector2.zero;
            _rigidbody.velocity = Vector2.zero;
        }

    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        Debug.Log(_health);

    }
}
