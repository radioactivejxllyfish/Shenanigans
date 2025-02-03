using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GruntEnemyBehaviour : EnemyBehaviour
    {
        private void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _maxhealth = 100f;
            _speed = 3.5f;
            _health = _maxhealth;
            _tookdmg = true;
            scoreRate = 1;
            cashRate = 5;
            
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthBar = GetComponentInChildren<Slider>();
            _detector = GetComponent<CircleCollider2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }


        private void Update()
        {
            Move();
            DeathCheck();
            Healthbar();
        }
    }
}