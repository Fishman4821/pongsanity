using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float impulse = 10f;
    public float drag = 0.005f;
    public float boost = 0.001f;
    private Rigidbody2D rb;
    private TrailRenderer tr;
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
        tr.emitting = false;
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
        tr = GetComponent<TrailRenderer>();
        Invoke("Serve", 2f);
    }


    void Update()
    {
        if (rb.velocity.magnitude < impulse && rb.velocity.magnitude > 0)
        {
            Debug.Log(rb.velocity.magnitude + " velocity.magnitude < impulse");
        }
        else
        {
            Debug.Log(rb.velocity.magnitude);
        }

        if (rb.velocity.magnitude > impulse + boost)
        {
            Vector2 dragForce = -rb.velocity.normalized * drag;
            if (rb.velocity.magnitude - (dragForce * Time.deltaTime).magnitude < impulse)
            {
                dragForce *= Time.deltaTime;
                dragForce -= dragForce.normalized * (rb.velocity.magnitude - impulse);
            }
            rb.velocity += dragForce;
        }
        else if (rb.velocity.magnitude < impulse)
        {
            Vector2 boostForce = rb.velocity.normalized * boost;
            if (rb.velocity.magnitude + (boostForce * Time.deltaTime).magnitude > impulse)
            {
                boostForce *= Time.deltaTime;
                boostForce -= boostForce.normalized * (rb.velocity.magnitude + boostForce.magnitude - impulse);
            }
            rb.velocity += boostForce;
        }

        if (rb.velocity.magnitude > impulse + boost * 2)
        {
            tr.emitting = true;
            tr.time = Math.Abs(rb.velocity.magnitude - impulse) / 50f; 
        }
        else
        {
            tr.emitting = false;
        }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Launcher"))
        {
            collision.gameObject.GetComponent<Launcher>().holdingBall = true;
            rb.velocity = Vector2.zero;
            transform.position = collision.gameObject.transform.position;
        }
    }
}
