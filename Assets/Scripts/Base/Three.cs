using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Three : Base
{
    public float speed = 3.0f;
    public float ultSpeedMult = 3.0f;
    public float ultBallMult = 2.0f;
    public override float UltDuration { get; set; } = 5.0f;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;

    protected int pos = 0;

    protected override void Init()
    {
        transform.localScale = new Vector3(0.25f, 2, 1);
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
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    protected void UpdatePos()
    {
        if (transform.position.y < pos * 2.0f - 0.05f)
        {
            rb.velocity = new Vector2(0, speed * (UltActive ? ultSpeedMult : 1f));
        }
        else if (transform.position.y > pos * 2.0f + 0.05f)
        {
            rb.velocity = new Vector2(0, -speed * (UltActive ? ultSpeedMult : 1f));
        }
        else
        {
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(transform.position.x, pos * 2.0f, 0);
        }
    }

    protected void MoveUp()
    {
        pos = Mathf.Clamp(pos + 1, -1, 1);
    }

    protected void MoveDown()
    {
        pos = Mathf.Clamp(pos - 1, -1, 1);
    }

    void Update()
    {
        if (UpKeyDown())
        {
            MoveUp();
        }
        else if (DownKeyDown())
        {
            MoveDown();
        }
        UpdatePos();
        UpdateUlt();
    }

    public override void ResetUlt()
    {
        UltActive = false;
        UltTimer = 0f;
        sr.color = Color.white;
    }

    public override void Ult()
    {
        UltMeter = 0f;
        UltActive = true;
        sr.color = Color.cyan;
    }
}
