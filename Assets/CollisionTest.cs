using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private CircleCollider2D _circleCollider2D;
    void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Hit");
            }
            else if (!other.gameObject.CompareTag("Objective") && !other.gameObject.CompareTag("PlayerRB") && !other.gameObject.CompareTag("Insertion") && !other.gameObject.CompareTag("Loot"))
            {
                Debug.Log("Player");

            }
        }
    }
}
