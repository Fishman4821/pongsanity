using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public bool Opponent;
    public float speed = 10f;
    public float boundY = 2.625f;
    protected SpriteRenderer sr;
    protected BoxCollider2D bc;
    protected Rigidbody2D rb;
    protected GameObject ball;
    protected bool holdingBall = false;

    void InitShape()
    {
        GameObject[] parts = new GameObject[5];
        Sprite sprite = Resources.Load<Sprite>("Square");
        for (int i = 0; i < 5; i++)
        {
            parts[i] = new GameObject("Part " + i);
            parts[i].transform.parent = transform;
            parts[i].AddComponent<SpriteRenderer>().sprite = sprite;
        }
        parts[0].transform.localPosition = new Vector3(0.125f + 0.125f / 4f - 0.01f, 0.75f / 2f - 0.125f / 4f, 1);
        parts[0].transform.localScale = new Vector3(0.75f / 2f, 0.125f / 2f, 1);

        parts[1].transform.localPosition = new Vector3(-0.125f / 2f, 0.75f / 2f - 0.125f / 2f - 0.005f, 1);
        parts[1].transform.localRotation = Quaternion.Euler(0, 0, -45);
        parts[1].transform.localScale = new Vector3(0.125f / 2f, 0.125f, 1);

        parts[2].transform.localPosition = new Vector3(-0.75f / 4f + 0.125f / 2f + 0.125f / 4f, 0, 1);
        parts[2].transform.localScale = new Vector3(0.125f / 2f, 0.5f + 0.125f / 2f, 1);

        parts[3].transform.localPosition = new Vector3(-0.125f / 2f, -0.75f / 2f + 0.125f / 2f + 0.005f, 1);
        parts[3].transform.localRotation = Quaternion.Euler(0, 0, 45);
        parts[3].transform.localScale = new Vector3(0.125f / 2f, 0.125f, 1);

        parts[4].transform.localPosition = new Vector3(0.125f + 0.125f / 4f - 0.01f, -0.75f / 2f + 0.125f / 4f, 1);
        parts[4].transform.localScale = new Vector3(0.75f / 2f, 0.125f / 2f, 1);
    
        parts[0].AddComponent<BoxCollider2D>();
        parts[2].AddComponent<BoxCollider2D>().size = new Vector2(1, 1.34f);
        parts[4].AddComponent<BoxCollider2D>();
    }

    void Start()
    {
        if (Opponent)
        {
            transform.position = new Vector3(4f, 0, 0);
        }
        else
        {
            transform.position = new Vector3(-4f, 0, 0);
        }
        InitShape();
        bc = gameObject.AddComponent<BoxCollider2D>();
        bc.size = new Vector2(0.75f / 2f, 0.75f - 0.125f);
        bc.offset = new Vector2(0.125f + 0.125f / 4f - 0.125f / 8f, 0);
        bc.isTrigger = true;
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        ball = GameObject.FindWithTag("Ball");
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
