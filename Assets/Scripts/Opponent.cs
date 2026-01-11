using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public float speed = 10f;
    public float boundY = 2.25f;
    protected Rigidbody2D rb;
    public GameObject ball;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
