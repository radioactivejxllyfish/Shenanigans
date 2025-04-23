using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class AIAgentTest : MonoBehaviour
{
    private AIPath path;
    public float moveSpeed;
    public Transform target;

    void Start()
    {
        path = GetComponent<AIPath>();
    }

    private void Update()
    {
        
        path.maxSpeed = moveSpeed;
        path.destination = target.position;
    }
}
