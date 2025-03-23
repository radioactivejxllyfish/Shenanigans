using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public static InventoryManager Instance { get; } = new InventoryManager();
    private Dictionary<ItemType, int> itemCounts = new Dictionary<ItemType, int>();




    public void AddItem(ItemType itemType, int amount)
    {
        if (itemCounts.ContainsKey(itemType))
        {
            itemCounts[itemType] += amount;
        }
        else
        {
            itemCounts[itemType] = amount;

        }
    }


    public void RemoveItem(ItemType itemType, int amount)
    {
        if (itemCounts.ContainsKey(itemType))
        {
            itemCounts[itemType] -= amount;
            if (itemCounts[itemType] <= 0)
                itemCounts.Remove(itemType);
        }
    }
    
    public int GetItemCount(ItemType itemType)
    {
        return itemCounts.TryGetValue(itemType, out int count) ? count : 0;
    }

}
