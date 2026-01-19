using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : Base
{
    public float speed = 7.5f;
    public float boundY = 2.5f;
    public override float UltDuration { get; set; } = 5.0f;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;

    void Start()
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
        if (Input.GetKey(KeyCode.W))
        {
            velocity.y = speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity.y = -speed;
        }
        else
        {
            velocity.y = 0;
        }
        if (UltActive)
        {
            if (Input.GetKey(KeyCode.A))
            {
                velocity.x = -speed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                velocity.x = speed;
            }
            else
            {
                velocity.x = 0;
            }
        } 
        else
        {
            if (transform.position.x > -3.95f || transform.position.x < -4.05f)
            {
                transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, Opponent ? 4f : -4f, speed * Time.deltaTime), transform.position.y, 0);
            }
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

    void UpdateUlt()
    {
        if (UltActive)
        {
            UltTimer += Time.deltaTime;
            if (UltTimer >= UltDuration)
            {
                ResetUlt();
            }
        }
        
        if (!UltActive && UltMeter < UltMeterMax)
        {
            UltMeter += Time.deltaTime * 0.5f;
        }

        if (UltMeter >= UltMeterMax && !UltActive && Input.GetKeyDown(KeyCode.Space))
        {
            Ult();
        }
    }

    public void ResetUlt()
    {
        UltActive = false;
        UltTimer = 0f;
        sr.color = Color.white;
    }

    public override void Ult()
    {
        UltActive = true;
        sr.color = Color.yellow;
    }
}
