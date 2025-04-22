using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemAssets : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public bool isConsumable;
    public int healAmount;
    public int addArmor;
    public int restoreStamina;
    public int addThrowables;
    public Sprite sprite;
    public string description;
    public int basePrice;
    
}
