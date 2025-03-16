using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public GameObject ui;
    public PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (ui.activeInHierarchy)
            {
                playerController.underUI = false;
                ui.SetActive(false);
            }
            else
            {
                playerController.underUI = true;
                ui.SetActive(true);
            }
        }
    }
}
