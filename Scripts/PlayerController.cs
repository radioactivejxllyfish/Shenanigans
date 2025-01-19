using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : PlayerVarPool
{

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        speed = 5f;
        health = 100f;
        stamina = 5f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerRb.AddForce(movement.normalized * 1.7f);
        }
        Move();
    }

    public override void Move()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        movement = new Vector2(xAxis, yAxis);
        playerRb.velocity = movement.normalized * speed;
    }
}
