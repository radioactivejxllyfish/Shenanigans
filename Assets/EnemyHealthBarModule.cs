using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBarModule : MonoBehaviour
{
    private float _health;
    private float _maxhealth;

    private GameObject _target;
    private ValueDumper _script;
    private Slider _slider;
    
    void Start()
    {
        _script = _target.GetComponentInParent<ValueDumper>();
        _slider = GetComponent<Slider>();
    }

    void Update()
    {
        _maxhealth = _script.maxhealthdump;
        _health = _script.healthdump;
        _slider.value = _health / _maxhealth;
        Mathf.Clamp(_health, 0f, _maxhealth);
    }
}
