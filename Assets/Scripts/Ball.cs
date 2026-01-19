using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float impulse = 10f;
    public float drag = 0.005f;
    public float boost = 0.001f;
    public Double parentDouble = null;
    public List<Double> extraDoubles = null;
    private Rigidbody2D rb;
    private TrailRenderer tr;
    private bool serveDirection = true; // false = right, true = left
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
        tr.emitting = false;
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    void Restart()
    {
        Reset();
        serveDirection = !serveDirection;
        // if (extraDoubles != null && extraDoubles.Count > 0)
        // {
        //     List<GameObject> extras = CreateExtra();
        //     for (int i = 0; i < extras.Count; i++)
        //     {

        //         StartCoroutine(extras[i].GetComponent<Ball>().Serve());
        //     }
        // }
        StartCoroutine(Serve());
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        extraDoubles = new List<Double>();
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
        if (collision.gameObject.CompareTag("Goal"))
        {
            // if (parentDouble != null)
            // {
            //     parentDouble.ResetUlt();
            //     Destroy(gameObject);
            // }
            // else
            // {
            //     Restart();
            // }
            Restart();
            return;
        }

        //if (!collision.gameObject.CompareTag("Ball"))
        if (!collision.gameObject.CompareTag("Wall") && !collision.gameObject.CompareTag("Bottom Double"))
        {
            Vector2 velocity;
            velocity.x = rb.velocity.x;
            velocity.y = (rb.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);
            rb.velocity = velocity;
        } 
        else if (collision.gameObject.CompareTag("Bottom Double"))
        {
            Vector2 velocity;
            velocity.x = rb.velocity.x;
            velocity.y = (rb.velocity.y / 2) + (-collision.collider.gameObject.GetComponent<BottomDouble>().topRb.velocity.y / 3);
            rb.velocity = velocity;
        }

        if (collision.gameObject.TryGetComponent<Base>(out var obj))
        {
            if (obj is Three three && three.UltActive)
            {
                rb.velocity = impulse * three.ultBallMult * rb.velocity.normalized;
            }
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

    // public void SpawnExtra(Double parent)
    // {
    //     extraDoubles.Add(parent);
    // }

    // private List<GameObject> CreateExtra()
    // {
    //     List<GameObject> extras = new();
    //     Debug.Log(extraDoubles.Count + " extraDoubles.Count");
    //     for (int i = 0; i < extraDoubles.Count; i++)
    //     {
    //         GameObject ball = Instantiate(gameObject, new Vector3(0, i / (extraDoubles.Count + 1) * 6, 0), Quaternion.identity);
    //         Ball ballScript = ball.GetComponent<Ball>();
    //         ballScript.name = "Extra Ball";
    //         ballScript.parentDouble = extraDoubles[i];
    //         ballScript.extraDoubles.Clear();
    //         extras.Add(ball);
    //     }
    //     extraDoubles.Clear();
    //     return extras;
    // }
}
