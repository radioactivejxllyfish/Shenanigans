using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericSword : MonoBehaviour
{
    public GameObject _player;
    public float _cooldown;
    public float _lightdamage;
    public float _heavydamage;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public abstract void LightAttack();
    public abstract void HeavyAttack(); 
}
