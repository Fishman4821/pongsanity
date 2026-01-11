using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float impulse = 10f;
    private Rigidbody2D rb;
    private bool serveDirection = true; // false = right, true = left
    void Serve()
    {
        float angle = UnityEngine.Random.Range(-45, 45);
        if (serveDirection)
        {
            angle += 180;
        }
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        rb.velocity = direction.normalized * impulse;
        serveDirection = !serveDirection;
    }

    void Reset()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    void Restart()
    {
        Reset();
        serveDirection = true;
        Invoke("Serve", 2f);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("Serve", 2f);
    }

    
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 velocity;
            velocity.x = rb.velocity.x;
            velocity.y = (rb.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);
            rb.velocity = velocity;
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            Restart();
        }
    }
}
