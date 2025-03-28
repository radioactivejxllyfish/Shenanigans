using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text itemName;
    public bool visible = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (visible)
        {
            itemName.alpha = 90;
        }
        else
        {
            itemName.alpha = 0;
        }
        
    }

    public void ShowItemName(string name, string rarity)
    {
        itemName.text = name;
        switch (rarity)
        {
            case "common":
                itemName.color = Color.green;
                break;
            case "rare":
                itemName.color = Color.blue;
                break;
            case "epic":
                itemName.color = Color.magenta;
                break;
            case "legendary":
                itemName.color = Color.red;
                break;
        }
    }
}
