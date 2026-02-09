using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : Base
{
    public float speed = 7.5f;
    public float boundY = 2.5f;
    public float ultForceMult = 2f;
    public override float UltDuration { get; set; } = 0.1f;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;

    protected override void Init()
    {
        transform.localScale = new Vector3(0.25f, 1, 1);
        if (Opponent)
        {
            transform.position = new Vector3(4f, 0, 0);
        }
        else
        {
            transform.position = new Vector3(-4f, 0, 0);
        }
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Square");
        gameObject.AddComponent<BoxCollider2D>();
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        Vector2 velocity = rb.velocity;
        if (UpKey())
        {
            velocity.y = speed;
        }
        else if (DownKey())
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
        if (Opponent)
        {
            if (pos.x > 5f)
            {
                pos.x = 5f;
            } 
            else if (pos.x < 1f)
            {
                pos.x = 1f;
            }
        }
        else
        {
            if (pos.x < -5f)
            {
                pos.x = -5f;
            } 
            else if (pos.x > -1f)
            {
                pos.x = -1f;
            }
        }
        transform.position = pos;

        UpdateUlt();
    }

    public override void ResetUlt()
    {
        UltActive = false;
    }

    public override void Ult()
    {
        UltMeter = 0f;
        UltActive = true;
        ball.GetComponent<Ball>().PaddleUlt(ultForceMult);
        ResetUlt();
    }
}
