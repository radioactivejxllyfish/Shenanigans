using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DefaultNamespace
{
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        public GameObject _player;
        public Rigidbody2D _rigidbody;
        public CircleCollider2D _detector;
        public SpriteRenderer _renderer;

        public int cashRate;
        public int scoreRate;
        public float _speed;
        public float _health;
        public float _maxhealth;
        public bool _tookdmg = true;
        public bool isDead = false;

        public Vector2 direction;
        public Slider _healthBar;

        public GameManager _gameManager;

        public void Healthbar()
        {
            _healthBar.value = _health / _maxhealth;
            Mathf.Clamp(_health, 0f, _maxhealth);
        }
        public void Move()
        {
            if (_player != null && !isDead)
            {
                direction = _player.transform.position - transform.position;
                _rigidbody.velocity = direction.normalized * _speed;
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
            }
        }
        
        public void TakeDamage(float damage)
        {
            _health -= damage;
            _tookdmg = true;
        }
        
        public void DeathCheck()
        {
            if (_health <= 50f)
            {
                _speed = 2.5f;
            }
            if (_health <= 0f && !isDead)
            {
                _speed = 0f;
                _renderer.color = Color.black;
                _gameManager.cash += cashRate;
                _gameManager.addScore(scoreRate);
                Destroy(gameObject, 1f);
                isDead = true;
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
            }

        }
    }
    
}