using System;
using UnityEngine;

public class PlayerController : PlayerVarPool
{

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        speed = 5f;
        health = 100f;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(xAxis, yAxis);
        playerRb.velocity = movement.normalized * speed;
    }
}
