using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject meleeInt;
    public GameObject rangedInt;

    public GameObject melee;
    public GameObject ranged;
    public BasicRifle basicRifle;
    public BasicSword basicSword;

    public float switchCooldown;
    public bool meleeActive;
    public bool rangedActive;
    
    
    void Start()
    {
        melee = Instantiate(meleeInt, transform.position, transform.rotation);
        ranged = Instantiate(rangedInt, transform.position, transform.rotation);
        basicRifle = ranged.GetComponent<BasicRifle>();
        basicSword = melee.GetComponent<BasicSword>();
        meleeInt.SetActive(true);
        rangedInt.SetActive(false);
        meleeActive = true;
        rangedActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && meleeActive && basicSword.canAttack)
        {
            meleeActive = false;
            rangedActive = true;
            ranged.SetActive(true);
            melee.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && rangedActive && !basicRifle.isReloading && basicRifle.canFire)
        {
            meleeActive = true;
            rangedActive = false;
            ranged.SetActive(false);
            melee.SetActive(true);
        }
    }
    
}
