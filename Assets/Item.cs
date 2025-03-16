using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{   
    public ItemType type;
    public Sprite image;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public bool stackable = true;


    public enum ItemType
    {
        Consumable,
        CraftingMaterial,
        Collectible,
    }

    public enum ActionType
    {
        Consume,
        Craft,
        Throw,
    }
}
