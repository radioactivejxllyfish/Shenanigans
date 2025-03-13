using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ObjectiveDefusal : MonoBehaviour
{


    [SerializeField] private GameObject _ui;
    [SerializeField] private Image _knot;
    [SerializeField] private Slider _slider;
    [SerializeField] private Slider _hit;
    
    private CircleCollider2D circle;
    private GameObject _player;
    
    private bool _switch = false;
    private bool _canScore = true;
    private bool _defused = false;
    private bool _failed = false;
    private float speed = 1f;
    private float randomChance;
    private int points;
    
    void Start()
    {
        circle = GetComponent<CircleCollider2D>();
        _ui.SetActive(false);
        Init();
    }

    
    void Update()
    {
        if (_player != null && Input.GetKeyDown(KeyCode.F))
        {
            _ui.SetActive(true);
        }
        if (_ui.gameObject.activeInHierarchy)
        {
            MainFunc();
            SlideRunner();
            if (points >= 5)
            {
                Destroy(gameObject);
                _defused = true;
            }
            else if (points <= -5)
            {
                _failed = true;
                StartCoroutine(Failed());
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
            float tolerance = _hit.value * 0.25f;
            if (Mathf.Abs(_slider.value - _hit.value) <= tolerance && _canScore)
            {
                _canScore = false;
                StartCoroutine(PointSetter());
                points += 1;
            }
            else if (Mathf.Abs(_slider.value - _hit.value) >= tolerance || !_canScore)
            {
                _canScore = false;
                points -= 1;
                StartCoroutine(PointSetter());
            }
        }
    }
    private IEnumerator PointSetter()
    {
        _canScore = false;
        _knot.color = Color.red;
        randomChance = Random.Range(0.12f, 0.88f);
        _hit.value = randomChance;
        yield return new WaitForSeconds(Random.Range(0.75f,2.25f));
        _canScore = true;
        _knot.color = Color.yellow;
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
        }
    }

    private IEnumerator Failed()
    {
        if (_player != null)
        {
            _player.GetComponent<PlayerVarPool>().TakeDamage(400f);
            Debug.Log(_player.GetComponent<PlayerVarPool>().health);
            
        }
        
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
