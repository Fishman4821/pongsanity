using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Double : Base
{
    public float speed = 6f;
    public float boundY = 2.5f;
    public override float UltDuration { get; set; } = 0f;
    protected GameObject topObj;
    protected SpriteRenderer topSr;
    protected Rigidbody2D topRb;
    
    protected GameObject bottomObj;
    protected SpriteRenderer bottomSr;

    protected override void Init()
    {
        topObj = new GameObject("TopPaddle");
        bottomObj = new GameObject("BottomPaddle");
        topObj.transform.parent = transform;
        bottomObj.transform.parent = transform;
        topObj.transform.localScale = new Vector3(0.25f, 1, 1);
        bottomObj.transform.localScale = new Vector3(0.25f, 1, 1);
        if (Opponent)
        {
            topObj.transform.position = new Vector3(4f, 1.5f, 0);
            bottomObj.transform.position = new Vector3(4f, -1.5f, 0);
        }
        else
        {
            topObj.transform.position = new Vector3(-4f, 1.5f, 0);
            bottomObj.transform.position = new Vector3(-4f, -1.5f, 0);
        }
        topSr = topObj.AddComponent<SpriteRenderer>();
        bottomSr = bottomObj.AddComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>("Square");
        topSr.sprite = sprite;
        bottomSr.sprite = sprite;
        topObj.AddComponent<BoxCollider2D>();
        bottomObj.AddComponent<BoxCollider2D>();
        topRb = topObj.AddComponent<Rigidbody2D>();
        topRb.gravityScale = 0;
        topRb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        topRb.bodyType = RigidbodyType2D.Kinematic;
        topRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        BottomDouble bottom = bottomObj.AddComponent<BottomDouble>();
        bottom.topRb = topRb;
        bottomObj.tag = "Bottom Double";
        topObj.tag = "Top Double";
    }

    protected void ClampYPosition()
    {
        Vector3 pos = topObj.transform.position;
        if (pos.y > boundY)
        {
            pos.y = boundY;
        }
        else if (pos.y < 0.5f)
        {
            pos.y = 0.5f;
        }
        topObj.transform.position = pos;
        bottomObj.transform.position = new Vector3(bottomObj.transform.position.x, -pos.y, bottomObj.transform.position.z);
    }

    void Update()
    {
        Vector2 velocity = topRb.velocity;
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
        topRb.velocity = velocity;

        ClampYPosition();

        UpdateUlt();
        if (ball.GetComponent<Ball>().scoreValue == 1 && UltActive)
        {
            ResetUlt();
        }
    }

    public override void ResetUlt()
    {
        UltActive = false;
    }

    public override void Ult()
    {
        UltMeter = 0f;
        UltActive = true;
        ball.GetComponent<Ball>().IncreaseScoreValue();
    }
}
