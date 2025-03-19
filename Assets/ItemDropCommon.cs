using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDropCommon : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] commonItemAssort;
    private CircleCollider2D _circleCollider2D;
    public int x;
    public int totalAmount;
    public int pickupAmount;
    public void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        totalAmount = Random.Range(1, 12);
    }

    public void Update()
    {
        
    }

    public void PickupItem(int id)
    {
        inventoryManager.AddItem(commonItemAssort[id]);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < totalAmount; i++)
        {
            x = Random.Range(1, 4);
            PickupItem(x);
        }
    }
}