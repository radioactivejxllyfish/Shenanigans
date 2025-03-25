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
        }
    }
    
    private void Heal(int amount)
    {
        Debug.Log($"Healed for {amount} HP");
        playerController.health += amount;
    }

    private void AddArmor(int amount)
    {
        Debug.Log($"Gained {amount} armor");
        playerController.health += amount;
    }

    private void RestoreStamina(int amount)
    {
        playerController.stamina += amount;
    }
    
    
}