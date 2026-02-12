using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Base
{
    public float speed = 6f;
    public float turnSpeed = 400f;
    public float boundY = 2.625f;
    public float ultForceMult = 2.5f;
    public override float UltDuration { get; set; } = 5.0f;
    public override float UltCharge { get; set; } = 0.125f;
    public bool holdingBall = false;
    protected BoxCollider2D bc;
    protected Rigidbody2D rb;
    protected GameObject[] parts;
    private float launchTimer = 0f;
    public float launchTimeLimit = 5f;

    void InitShape()
    {
        parts = new GameObject[5];
        Sprite sprite = Resources.Load<Sprite>("Square");
        for (int i = 0; i < 5; i++)
        {
            parts[i] = new GameObject("Part " + i);
            parts[i].transform.parent = transform;
            parts[i].AddComponent<SpriteRenderer>().sprite = sprite;
        }
        parts[0].transform.localPosition = new Vector3(0.125f + 0.125f / 4f - 0.01f - 0.25f, 0.75f / 2f - 0.125f / 4f, 1);
        parts[0].transform.localScale = new Vector3(0.75f / 2f, 0.125f / 2f, 1);

        parts[1].transform.SetLocalPositionAndRotation(new Vector3(-0.125f / 2f - 0.25f, 0.75f / 2f - 0.125f / 2f - 0.005f, 1), Quaternion.Euler(0, 0, -45));
        parts[1].transform.localScale = new Vector3(0.125f / 2f, 0.125f, 1);

        parts[2].transform.localPosition = new Vector3(-0.75f / 4f + 0.125f / 2f + 0.125f / 4f - 0.25f, 0, 1);
        parts[2].transform.localScale = new Vector3(0.125f / 2f, 0.5f + 0.125f / 2f, 1);

        parts[3].transform.SetLocalPositionAndRotation(new Vector3(-0.125f / 2f - 0.25f, -0.75f / 2f + 0.125f / 2f + 0.005f, 1), Quaternion.Euler(0, 0, 45));
        parts[3].transform.localScale = new Vector3(0.125f / 2f, 0.125f, 1);

        parts[4].transform.localPosition = new Vector3(0.125f + 0.125f / 4f - 0.01f - 0.25f, -0.75f / 2f + 0.125f / 4f, 1);
        parts[4].transform.localScale = new Vector3(0.75f / 2f, 0.125f / 2f, 1);

        if (Opponent)
        {
            for (int i = 0; i < 5; i++)
            {
                parts[i].transform.localPosition = new Vector3(-parts[i].transform.localPosition.x, parts[i].transform.localPosition.y, parts[i].transform.localPosition.z);
            }
            parts[1].transform.localRotation = Quaternion.Euler(0, 0, 45);
            parts[3].transform.localRotation = Quaternion.Euler(0, 0, -45);
        }

        parts[0].AddComponent<BoxCollider2D>();
        parts[2].AddComponent<BoxCollider2D>().size = new Vector2(1, 1.34f);
        parts[4].AddComponent<BoxCollider2D>();
    }

    protected override void Init()
    {
        gameObject.tag = "Launcher";
        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.size = new Vector2(0.75f / 2f, 0.75f - 0.125f);
        bc.isTrigger = true;
        if (Opponent)
        {
            transform.position = new Vector3(4f, 0, 0);
            bc.offset = new Vector2(0.125f - 0.125f / 4f + 0.125f / 8f, 0);
        }
        else
        {
            transform.position = new Vector3(-4f, 0, 0);
            bc.offset = new Vector2(0.125f + 0.125f / 4f - 0.125f / 8f - 0.25f, 0);
        }
        InitShape();
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        ball = GameObject.FindWithTag("Ball");
    }

    protected void ResetRotation()
    {
        if (transform.rotation.eulerAngles.z > 0.1f || transform.rotation.eulerAngles.z < -0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, 0, turnSpeed * Time.deltaTime));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    protected void Launch(float angleOffset = 0f)
    {
        ball.GetComponent<Rigidbody2D>().velocity = (UltActive ? ultForceMult : 1f) * ball.GetComponent<Ball>().impulse * new Vector2(Mathf.Cos((transform.rotation.eulerAngles.z + angleOffset) * Mathf.Deg2Rad), Mathf.Sin((transform.rotation.eulerAngles.z + angleOffset) * Mathf.Deg2Rad));
        holdingBall = false;
        if (UltActive)
        {
            ResetUlt();
        }
    }

    void Update()
    {
        if (holdingBall)
        {
            rb.velocity = Vector2.zero;
            float angVel;
            if (LeftKey())
            {
                angVel = turnSpeed;
            }
            else if (RightKey())
            {
                angVel = -turnSpeed;
            }
            else
            {
                angVel = 0;
            }
            rb.angularVelocity = angVel;
            ball.transform.position = transform.position;
            launchTimer += Time.deltaTime;
            if (launchTimer >= launchTimeLimit)
            {
                Launch();
                launchTimer = 0f;
            }
        }
        else
        {
            ResetRotation();

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
        }

        if (UpKeyUp() && holdingBall)
        {
            if (twoPlayer && Opponent)
            {
                Launch(180f);
            }
            else
            {
                Launch();
            }
        }

        UpdateUlt();
    }

    public override void ResetUlt()
    {
        UltActive = false;
        UltTimer = 0f;
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public override void Ult()
    {
        UltMeter = 0f;
        UltActive = true;
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
