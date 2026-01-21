using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaddleType;
using System;

public class UltMeter : MonoBehaviour
{
    public Character character;
    public Base player;
    public bool Opponent;
    public float height = 0.25f;

    private GameObject meter;
    private bool initialized = false;
    Color CharacterToColor(Character c)
    {
        return c switch
        {
            Character.Paddle => Color.yellow,
            Character.Double => Color.green,
            Character.Three => Color.cyan,
            Character.Launcher => Color.red,
            _ => Color.yellow
        };
    }

    void CreateShape(out GameObject obj, out SpriteRenderer sr, in string res, in Color color)
    {
        obj = new GameObject("Shape");
        obj.transform.parent = transform;
        sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>(res);
        sr.color = color;
        sr.sortingLayerName = "Background";
        sr.sortingOrder = 2;
    }

    public Color c1;
    public Color c2;

    void Background()
    {
        GameObject obj;
        SpriteRenderer sr;
        CreateShape(out obj, out sr, "Square", c1);
        obj.transform.localPosition = new Vector3(0, 0, 1);
        obj.transform.localScale = new Vector3(2.4f, 0.45f, 1);
        CreateShape(out obj, out sr, "Triangle", c1);
        obj.transform.localPosition = new Vector3(-1.2f, 0.076f * (Opponent ? 1 : -1), 1);
        obj.transform.localScale = new Vector3(0.2f, -0.515f * (Opponent ? 1 : -1), 1);
        CreateShape(out obj, out sr, "Triangle", c1);
        obj.transform.localPosition = new Vector3(1.2f, -0.076f * (Opponent ? 1 : -1), 1);
        obj.transform.localScale = new Vector3(-0.2f, 0.515f * (Opponent ? 1 : -1), 1);
        CreateShape(out obj, out sr, "Square", c2);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localScale = new Vector3(2.1f, 0.25f, 1);
        sr.sortingOrder = 3;
    }

    public void Init(GameObject obj, Character c)
    {
        character = c;
        if (Opponent)
        {
            player = obj.GetComponent<Base>();
        }
        else
        {
            player = obj.GetComponent<Base>();
        }     
        meter = new GameObject("UltMeter");
        meter.transform.parent = transform;
        SpriteRenderer sr = meter.AddComponent<SpriteRenderer>();   
        sr.sprite = Resources.Load<Sprite>("Square");
        sr.color = CharacterToColor(character);
        sr.sortingLayerName = "Background";
        sr.sortingOrder = 4;
        meter.transform.localPosition = new Vector3(0, 0, 0);
        Background();
        initialized = true;
    }

    void Update()
    {
        if (!initialized)
        {
            return;
        }

        if (player.UltActive)
        {
            if (player.UltDuration == 0f) 
            {
                meter.transform.localPosition = new Vector3(-1f + 1, meter.transform.localPosition.z);
                meter.transform.localScale = new Vector3(2.1f, height, 1f);
            } 
            else
            {
                meter.transform.localPosition = new Vector3(-1f + (player.UltDuration - player.UltTimer) / player.UltDuration, meter.transform.localPosition.y, meter.transform.localPosition.z);
                meter.transform.localScale = new Vector3((player.UltDuration - player.UltTimer) / player.UltDuration * 2f + 0.1f, height, 1f);
            }
        }
        else
        {
            meter.transform.localPosition = new Vector3(-1f + player.UltMeter / player.UltMeterMax, meter.transform.localPosition.y, meter.transform.localPosition.z);
            meter.transform.localScale = new Vector3(player.UltMeter / player.UltMeterMax * 2f + 0.1f, height, 1f);
        }
        if (Opponent)
        {
            meter.transform.localPosition = new Vector3(-meter.transform.localPosition.x, meter.transform.localPosition.y, meter.transform.localPosition.z);
        }
    }
}
