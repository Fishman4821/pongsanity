using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float impulse = 10f;
    public float drag = 0.005f;
    public float boost = 0.001f;
    public bool doubleScore = false;
    public ScoreController score;
    private Rigidbody2D rb;
    private TrailRenderer tr;
    private SpriteRenderer sr;
    private bool serveDirection = true; // false = right, true = left
    public AudioSource audioSource;
    public AudioClip bounceBonk;
    public AudioClip scoreDing;
    IEnumerator Serve()
    {
        yield return new WaitForSeconds(2f);
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
        doubleScore = false;
        sr.color = Color.white;
        tr.emitting = false;
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    void Restart()
    {
        Reset();
        StartCoroutine(Serve());
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Serve());
    }


    void Update()
    {
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
        if (!collision.gameObject.CompareTag("Wall") && !collision.gameObject.CompareTag("Bottom Double"))
        {
            Vector2 velocity;
            velocity.x = rb.velocity.x;
            velocity.y = (rb.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);
            rb.velocity = velocity;
            audioSource.PlayOneShot(bounceBonk);
        } 
        else if (collision.gameObject.CompareTag("Bottom Double"))
        {
            Vector2 velocity;
            velocity.x = rb.velocity.x;
            velocity.y = (rb.velocity.y / 2) + (-collision.collider.gameObject.GetComponent<BottomDouble>().topRb.velocity.y / 3);
            rb.velocity = velocity;
            audioSource.PlayOneShot(bounceBonk);
        }

        if (collision.gameObject.TryGetComponent<Base>(out var obj))
        {
            obj.IncrementUltMeter();
            if (obj is Three three && three.UltActive)
            {
                rb.velocity = impulse * three.ultBallMult * rb.velocity.normalized;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            if (collision.gameObject.transform.position.x < 0)
            {
                score.IncrementOpponentScore(doubleScore);
                audioSource.PlayOneShot(scoreDing);
            }
            else
            {
                score.IncrementPlayerScore(doubleScore);
                audioSource.PlayOneShot(scoreDing);
            }
            Restart();
            return;
        }
        else if (collision.gameObject.CompareTag("Launcher"))
        {
            Launcher l = collision.gameObject.GetComponent<Launcher>();
            l.holdingBall = true;
            l.IncrementUltMeter();
            rb.velocity = Vector2.zero;
            transform.position = collision.gameObject.transform.position;
        }
    }

    public void PaddleUlt(float mult)
    {
        rb.velocity = impulse * mult * -rb.velocity.normalized;
        rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);  
    }

    public void DoubleScore()
    {
        doubleScore = true;
        sr.color = Color.green;
    }
}
