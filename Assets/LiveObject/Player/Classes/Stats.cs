using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject player;
    public string ChosenClass;
    public string InherentSkill;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        
        ChosenClass = playerController.Class;
        switch (ChosenClass)
        {
            case "U11_Liquidator":
                playerController = GetComponent<PlayerController>();
                playerController.maxHealth = 180f;
                playerController.health = playerController.maxHealth;
                playerController.MAX_STAMINA = 100;
                playerController.stamina = playerController.MAX_STAMINA;
                playerController.speed = 5f;
                playerController._dashpower = 7.5f;
                playerController.grenadeCount = 3;
                
                playerController.Resistance_Melee = 1.0f;
                playerController.Resistance_Ranged = 1.0f;
                playerController.Resistance_Explosive = 1.0f;
                playerController.Resistance_Energy = 2f;
                
                playerController.Multiplier_MeleeDamage = 1.0f;
                playerController.Multiplier_RangedDamage = 1.0f;
                playerController.Multiplier_SkillDamage = 1.0f;
                playerController.Multiplier_UltimateDamage = 1.0f;

                InherentSkill = "CombatMode";
                break;
            case "U11_Grenadier":
                playerController = GetComponent<PlayerController>();
                playerController.maxHealth = 150f;
                playerController.health = playerController.maxHealth;
                playerController.MAX_STAMINA = 130;
                playerController.stamina = playerController.MAX_STAMINA;
                playerController.speed = 4.5f;
                playerController._dashpower = 7f;
                playerController.grenadeCount = 8;
                
                playerController.Resistance_Melee = 0.9f;
                playerController.Resistance_Ranged = 0.9f;
                playerController.Resistance_Explosive = 1.1f;
                playerController.Resistance_Energy = 2f;
                
                playerController.Multiplier_MeleeDamage = 0.75f;
                playerController.Multiplier_RangedDamage = 1.0f;
                playerController.Multiplier_SkillDamage = 1.35f;
                playerController.Multiplier_UltimateDamage = 1.0f;
                
                InherentSkill = "MortarRain";
                break;
            case "U11_SpecialRecon":
                playerController = GetComponent<PlayerController>();
                playerController.maxHealth = 80f;
                playerController.health = playerController.maxHealth;
                playerController.MAX_STAMINA = 200;
                playerController.stamina = playerController.MAX_STAMINA;
                playerController.speed = 6f;
                playerController._dashpower = 8f;
                playerController.grenadeCount = 2;
                
                playerController.Resistance_Melee = 1.0f;
                playerController.Resistance_Ranged = 0.85f;
                playerController.Resistance_Explosive = 0.85f;
                playerController.Resistance_Energy = 2f;
                
                playerController.Multiplier_MeleeDamage = 1.75f;
                playerController.Multiplier_RangedDamage = 1.25f;
                playerController.Multiplier_SkillDamage = 1.55f;
                playerController.Multiplier_UltimateDamage = 0.75f;
                
                InherentSkill = "DeadShot";
                break;
            case "U11_EOD":
                playerController = GetComponent<PlayerController>();
                playerController.maxHealth = 250f;
                playerController.health = playerController.maxHealth;
                playerController.MAX_STAMINA = 75;
                playerController.stamina = playerController.MAX_STAMINA;
                playerController.speed = 4.75f;
                playerController._dashpower = 7f;
                playerController.grenadeCount = 3;
                
                playerController.Resistance_Melee = 0.75f;
                playerController.Resistance_Ranged = 0.75f;
                playerController.Resistance_Explosive = 0.45f;
                playerController.Resistance_Energy = 1f;
                
                playerController.Multiplier_MeleeDamage = 1.45f;
                playerController.Multiplier_RangedDamage = 1f;
                playerController.Multiplier_SkillDamage = 1f;
                playerController.Multiplier_UltimateDamage = 1f;
                break;
                
            case null:
                throw new NotImplementedException();
        }
    }

    void Update()
    {
        
    }


    public void SKill_CombatMode()
    {
        
    }
}
