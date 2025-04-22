using System;
using UnityEngine;

public class ItemUsageHandler : MonoBehaviour
{
    public PlayerController playerController;


    private void Awake()
    {
        if (playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("PlayerRB").GetComponent<PlayerController>();
        }
    }

    public void UseItem(ItemAssets item)
    {
        if (item.isConsumable)
        {
            if (item.healAmount > 0)
            {
                Heal(item.healAmount);
            }
            if (item.addArmor > 0)
            {
                AddArmor(item.addArmor);
            }
            if (item.restoreStamina > 0)
            {
                RestoreStamina(item.restoreStamina);
            }

            if (item.addThrowables > 0)
            {
                AddThrowables(item.addThrowables);
            }
        }
    }
    
    private void Heal(int amount)
    {
        if (playerController.health < playerController.maxHealth)
        {
            playerController.health += amount;
        }
    }

    private void AddArmor(int amount)
    {
        if (playerController.armorCount < playerController.maxArmorCount)
        {
            playerController.armorCount += amount;
        }
    }
    
    private void AddThrowables(int amount)
    {
        playerController.grenadeCount += amount;
    }

    private void RestoreStamina(int amount)
    {
        if (playerController.stamina < playerController.MAX_STAMINA)
        {
            playerController.stamina += amount;
        }
    }
    
    
}