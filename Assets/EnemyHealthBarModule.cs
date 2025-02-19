using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBarModule : MonoBehaviour
{
    private float _health;
    private float _maxhealth;

    private GameObject _target;
    private BasicEnemy _script;
    private Slider _slider;
    
    void Start()
    {
        _script = GetComponentInParent<BasicEnemy>();
        _slider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        Debug.Log(_health);
        _maxhealth = _script.maxhealth;
        _health = _script.health;
        _slider.value = _health / _maxhealth;
        Mathf.Clamp(_health, 0f, _maxhealth);
    }
}
