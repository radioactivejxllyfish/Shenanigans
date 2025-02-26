using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMultiplier : MonoBehaviour
{
    public BasicSword basicSword;
    public BasicRifle basicRifle;
    public PlayerController playerController;
    public WeaponSelector weaponSelector;

    public float maxStaminaO;
    public float maxStamina;

    public float speedO;
    public float speed;
    
    public float maxHealthO;
    public float maxHealth;

    public float meleeDamageO;
    public float meleeDamage;
    
    public float rangedDamageO;
    public float rangedDamage;
    
    
    public float rangedFireRateO;
    public float rangedFireRate;
    
    public float rangedCapacityO;
    public float rangedCapacity;
    
    public float rangedReserveO;
    public float rangedReserve;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        weaponSelector = GetComponent<WeaponSelector>();
        basicRifle = weaponSelector.basicRifle;
        basicSword = weaponSelector.basicSword;
        
        
        maxStaminaO = playerController.MAX_STAMINA;
        speedO = playerController.speed;
        maxHealthO = playerController.maxHealth;
        meleeDamageO = basicSword.damage;
        rangedDamageO = basicRifle.damage;
        rangedFireRateO = basicRifle.fireRate;
        rangedCapacityO = basicRifle.magazineCapacity;
    }
    void Update()
    {
        
    }

    public void BuffMaxHealth(float multiplier)
    {
        maxHealth = maxHealthO * multiplier;
    }
    
    public void BuffSpeed(float multiplier)
    {
        speed = speedO * multiplier;
    }
    
    public void BuffMeleeDamage(float multiplier)
    {
        meleeDamage = meleeDamageO * multiplier;
    }
    
    public void BuffRangedDamage(float multiplier)
    {
        rangedDamage = rangedDamageO * multiplier;
    }
    
    public void BuffRangedFireRate(float multiplier)
    {
        rangedFireRate = rangedFireRateO * multiplier;
    }
    
    
}
