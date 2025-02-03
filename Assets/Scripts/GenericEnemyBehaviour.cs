using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GenericEnemyBehaviour : MonoBehaviour
{
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    private float _speed;
    private float _health;
    private float _maxhealth;
    private bool _tookdmg = true;
    private Vector2 direction;
    private CircleCollider2D _detector;
    private SpriteRenderer _renderer;
    private Slider _healthBar;
    
    
    private void Start()
    {
        _speed = 2.6f;
        _health = 100f;
        _maxhealth = 100f;
        _rigidbody = GetComponent<Rigidbody2D>();
        _healthBar = GetComponentInChildren<Slider>();
        _detector = GetComponent<CircleCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        StartCoroutine("TakingDamage");
    }

    private void Update()
    {
        _healthBar.value = _health / _maxhealth;
        Mathf.Clamp(_health, 0f, 100f);
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


    private IEnumerator TakingDamage()
    {
        while (true)
        {
            if (_tookdmg)
            {
                _renderer.color = Color.blue;
                yield return new WaitForSeconds(0.2f);
                _renderer.color = Color.red;
                _tookdmg = false;
            }

            yield return null;
        }
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        _tookdmg = true;
        Debug.Log(_health);
    }
}
