using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBar : MonoBehaviour
{

    public Slider slider;
    public Image image;
    public PlayerVarPool playerVarPool;
    
    public Slider stamina;

    private float healthValue;
    private float maxHealth;
    private float staminaValue;
    private float maxStamina;
    
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        playerVarPool = GameObject.FindGameObjectWithTag("PlayerRB").GetComponent<PlayerVarPool>();
    }

    void Update()
    {
        healthValue = playerVarPool.health;
        maxHealth = playerVarPool.maxHealth;
        staminaValue = playerVarPool.stamina;
        maxStamina = playerVarPool.MAX_STAMINA;

        stamina.value = staminaValue / maxStamina;
        slider.value = healthValue / maxHealth;
        
        if (slider.value <= maxHealth * 70 / 100)
        {
            image.color = Color.green;
        }
        else if (slider.value <= maxHealth * 50/100)
        {
            image.color = Color.yellow;
        }
        else if (slider.value <= maxHealth * 25 / 100)
        {
            image.color = Color.red;
        }
        
        
    }
}
