using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : PlayerVarPool
{
    private int stamina = 100;
    private static readonly byte MAX_STAMINA = 100;
    bool sprinting;
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        speed = 5f;
        health = 100f;
        StartCoroutine("Sprint");
    }
    
    private void Update()
    {
        Move();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }
            
    }


    public override void Move()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        movement = new Vector2(xAxis, yAxis);
        playerRb.velocity = movement.normalized * speed;
    }


    private IEnumerator Sprint()
    {
        while (true)
        {
            if (sprinting && stamina > 0 )
            {
                speed = 15f;
                stamina -= 2;
                if (stamina < 0) stamina = 0;
                Debug.Log("chay");
            }
            else if (!sprinting)
            {
                speed = 5f;
                stamina += 1;
                if (stamina > MAX_STAMINA) stamina = MAX_STAMINA;
                Debug.Log("hoi");
            }
            else if (stamina == 0)
            {
                speed = 0f;
                yield return new WaitForSeconds(3f);
            }
            staminaText.text = stamina.ToString();
            yield return new WaitForSeconds(0.1f);
        }


    }

    public Text staminaText;
}
