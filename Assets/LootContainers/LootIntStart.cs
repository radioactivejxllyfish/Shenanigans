using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootIntStart : MonoBehaviour
{
    public GameObject lootCommonMeds;
    public GameObject lootRareTokens;
    public float radius;
    public int amount;
    public Vector3 position;
    void Start()
    {
        radius = 22f;
        amount = Random.Range(5, 12);
        Spawn();
    }

    void Update()
    {

    }


    private void Spawn()
    {
        GameObject spawn;
        for (int i = 0; i < amount; i++)
        {
            position = new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), 0f);
            int x = Random.Range(1, 4);
            if (x == 1)
            {
                spawn = lootRareTokens;
            }
            else
            {
                spawn = lootCommonMeds;
            }
            Instantiate(spawn, position , Quaternion.Euler(0,0, Random.Range(0,360)));

        }

    }
    
}
