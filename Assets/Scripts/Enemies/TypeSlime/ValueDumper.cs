using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueDumper : EnemyTypeSlime
{
    public float maxhealthdump;
    public float healthdump;
    public float speeddump;
    public float damagedump;

    void Update()
    {
        healthdump = health;
        maxhealthdump = maxhealth;
        speeddump = speed;
        damagedump = damage;
    }
}