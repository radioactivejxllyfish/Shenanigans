using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Loot_MedicalSupply : MonoBehaviour
{

    private CircleCollider2D _circleCollider2D;
    private Rigidbody2D _rigidbody2D;
    private GameObject player;
    private InventoryManager _inventoryManager =InventoryManager.Instance;
    private int x;
    private int y;
    private int z;
    private int armor;
    private bool hasSight = false;
    private bool cursorAvailable = false;


    void Start()
    {
        
        _circleCollider2D = GetComponent<CircleCollider2D>();
        x = Random.Range(1, 5);
        y = Random.Range(0, 2);
        z = Random.Range(0, 2);
        armor = Random.Range(0, 2);
    }

    void Update()
    {
        SightText();
        if (hasSight)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Pickup();
                Destroy(gameObject, 0.05f);
            }
        }
    }


    private void SightText()
    {
        if (player != null)
        {
            if (hasSight)
            {
                player.GetComponent<UIManager>().ShowItemName("Medical Supply", "common");
                player.GetComponent<UIManager>().visible = true;
            }
            else
            {
                player.GetComponent<UIManager>().visible = false;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerRB"))
        {
            player = other.gameObject;
            if (cursorAvailable)
            {
                hasSight = true;
            }
        }


        if (other.gameObject.CompareTag("Cursor") && player != null)
        {
            cursorAvailable = true;
            hasSight = true;
        }
        if (other.gameObject.CompareTag("Cursor"))
        {
            cursorAvailable = true;
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerRB"))
        {
            player.GetComponent<UIManager>().visible = false;
            player = null;
            hasSight = false;
        }
        
        if (collider.gameObject.CompareTag("Cursor"))
        {
            cursorAvailable = false;
            hasSight = false;
        }
        
    }


    private void Pickup()
    {
        if (x != 0)
        {
            InventoryManager.Instance.AddItem(ItemType.Bandage, x);
        }

        if (y != 0)
        {
            InventoryManager.Instance.AddItem(ItemType.Medkit, y);
        }

        if (z != 0)
        {
            InventoryManager.Instance.AddItem(ItemType.EnergyDrink, z);

        }

        if (armor != 0)
        {
            InventoryManager.Instance.AddItem(ItemType.ArmorPlateSteel, armor);
        }
    }
}
