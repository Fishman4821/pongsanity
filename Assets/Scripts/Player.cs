using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public float speed = 10f;
    public float boundY = 2.25f;
    protected Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 velocity = rb.velocity;
        if (Input.GetKey(upKey))
        {
            velocity.y = speed;
        }
        else if (Input.GetKey(downKey))
        {
            velocity.y = -speed;
        }
        else
        {
            velocity.y = 0;
        }
        rb.velocity = velocity;

        Vector3 pos = transform.position;
        if (pos.y > boundY)
        {
            pos.y = boundY;
        }
        else if (pos.y < -boundY)
        {
            pos.y = -boundY;
        }
        transform.position = pos;
    }
}
