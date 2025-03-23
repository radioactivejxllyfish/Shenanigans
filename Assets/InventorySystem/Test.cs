using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private InventoryManager _inventoryManager =InventoryManager.Instance;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            InventoryManager.Instance.AddItem(ItemType.Bandage, 1);

            int potionCount = InventoryManager.Instance.GetItemCount(ItemType.Bandage);
            Debug.Log("Health Potions: " + potionCount);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            InventoryManager.Instance.RemoveItem(ItemType.Bandage, 1);
            int potionCount = InventoryManager.Instance.GetItemCount(ItemType.Bandage);
            Debug.Log("Health Potions: " + potionCount);
        }
    }
}
