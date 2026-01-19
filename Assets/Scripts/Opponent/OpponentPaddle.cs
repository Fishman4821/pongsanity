using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentPaddle : Paddle
{
    protected override bool Opponent { get; set; } = true;

    void Update()
    {
        Vector2 velocity = rb.velocity;
        if (ball.transform.position.y > transform.position.y + 0.1f)
        {
            velocity.y = speed;
        }
        else if (ball.transform.position.y < transform.position.y - 0.1f)
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
