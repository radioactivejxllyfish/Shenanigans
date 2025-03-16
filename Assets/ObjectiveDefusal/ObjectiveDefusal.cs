using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class ObjectiveDefusal : MonoBehaviour
{


    [SerializeField] private GameObject _ui;
    [SerializeField] private Image _knot;
    [SerializeField] private Slider _slider;
    [SerializeField] private Slider _hit;
    [SerializeField] private Image led;
    
    
    
    private SpriteRenderer sprite;
    private ParticleSystem particles;
    private List<GameObject> _enemies = new List<GameObject>();
    private CircleCollider2D circle;
    private GameObject _player;
    
    
    private bool _switch = false;
    private bool _canScore = true;
    private bool _defused = false;
    private bool _failed = false;
    private float speed = 1f;
    private float randomChance;
    private int points;
    private bool explode = false;
    
    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        particles = GetComponent<ParticleSystem>();
        circle = GetComponent<CircleCollider2D>();
        _ui.SetActive(false);
        Init();
    }

    
    void Update()
    {
        if (_player != null && Input.GetKeyDown(KeyCode.F) && Vector2.Distance(_player.transform.position, transform.position) < 3f)
        {
            _ui.SetActive(true);
            _player.GetComponent<PlayerController>().isStunned = true;
        }
        
        
        if (_ui.gameObject.activeInHierarchy)
        {
            MainFunc();
            SlideRunner();
            if (points >= 5)
            {
                _ui.SetActive(false);
                Destroy(gameObject, 8f);
                _defused = true;
                _player.GetComponent<PlayerController>().isStunned = false;
            }
            else if (points <= -5)
            {
                _failed = true;
                if (_failed && !explode)
                {
                    StartCoroutine(Failed());
                    
                }
                sprite.enabled = false;

            }
        }
    }

    private void Init()
    {
        StartCoroutine(PointSetter());
        _knot.color = Color.yellow;
    }
    private void MainFunc()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float tolerance = _hit.value * 0.22f;
            if (Mathf.Abs(_slider.value - _hit.value) <= tolerance && _canScore)
            {
                led.color = Color.green;
                _canScore = false;
                StartCoroutine(PointSetter());
                points += 1;
            }
            else if (Mathf.Abs(_slider.value - _hit.value) >= tolerance || !_canScore)
            {
                led.color = Color.red;
                _canScore = false;
                points -= 1;
                StartCoroutine(PointSetter());
                _player.GetComponent<PlayerVarPool>().cameraSmoother.CameraShake(0.3f,0.05f);
                
            }
        }
    }
    private IEnumerator PointSetter()
    {
        speed = Random.Range(0.85f, 2f);
        _canScore = false;
        _knot.color = Color.red;
        randomChance = Random.Range(0.12f, 0.88f);
        _hit.value = randomChance;
        yield return new WaitForSeconds(Random.Range(0.45f,1.25f));
        _canScore = true;
        _knot.color = Color.yellow;
        led.color = Color.white;
    }

    private void SlideRunner()
    {
        if (_slider.value == 1)
        {
            _switch = true;
        }
        else if (_slider.value == 0)
        {
            _switch = false;
        }
        
        if (!_switch)
        {
            _slider.value += Time.deltaTime * speed;
        }
        else if (_switch)
        {
            _slider.value -= Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("PlayerRB"))
            {
                _player = other.gameObject;
            }

            if (other.gameObject.CompareTag("Enemy"))
            {
                _enemies.Add(other.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D otherobject)
    {
        if (otherobject != null)
        {
            if (otherobject.gameObject.CompareTag("PlayerRB"))
            {
                _player = null;
            }
            if (otherobject.gameObject.CompareTag("Enemy"))
            {
                _enemies.Remove(otherobject.gameObject);
                
            }
        }
    }

    private IEnumerator Failed()
    {

        yield return new WaitForSeconds(0.05f);
        led.color = Color.yellow;
        yield return new WaitForSeconds(0.05f);
        led.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        led.color = Color.yellow;
        yield return new WaitForSeconds(0.05f);
        led.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        led.color = Color.yellow;
        yield return new WaitForSeconds(0.05f);
        led.color = Color.red;
        _ui.SetActive(false);
        _player.GetComponent<PlayerController>().isStunned = false;
        yield return new WaitForSeconds(1.7f);
        explode = true;
        particles.Play();
        if (_player != null)
        {
            _player.GetComponent<PlayerVarPool>().TakeDamage(400f);
            _player.GetComponent<PlayerVarPool>().cameraSmoother.CameraShake(0.1f,0.05f);
        }

        if (_enemies != null)
        {
            foreach (GameObject enemy in _enemies)
            {
                enemy.GetComponent<BasicEnemy>().TakeDamage(400f);
            }
        }
        
        yield return new WaitForSeconds(1f);
        Destroy(gameObject, 4f);
    }
}
