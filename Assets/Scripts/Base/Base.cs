using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
    public GameObject ball = null;
    public virtual bool Opponent { get; set; } = false;
    public bool twoPlayer = false;
    public float UltMeter = 0f;
    public float UltMeterMax = 1f;
    public virtual float UltDuration { get; set; }
    public float UltTimer = 0f;
    public bool UltActive = false;
    public virtual float UltCharge { get; set; } = 0.25f;
    public abstract void Ult();
    public abstract void ResetUlt();

    protected KeyCode upKey;
    protected KeyCode downKey;
    protected KeyCode leftKey;
    protected KeyCode rightKey;
    protected List<KeyCode> ultKey;

    protected abstract void Init();

    protected virtual void InitOpponent() { }

    protected void UpdateUlt()
    {
        if (UltActive && UltDuration != 0f)
        {
            UltTimer += Time.deltaTime;
            if (UltTimer >= UltDuration)
            {
                ResetUlt();
            }
        }

        if (((!twoPlayer && !Opponent) || twoPlayer) && UltKeyDown() && UltReady())
        {
            Ult();
        }
    }

    public void IncrementUltMeter()
    {
        if (!UltActive) UltMeter = Mathf.Clamp(UltMeter + UltCharge, 0f, UltMeterMax);
    }

    public bool UltReady()
    {
        return UltMeter >= UltMeterMax && !UltActive;
    }

    void Start()
    {
        if (twoPlayer && Opponent)
        {
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            ultKey = new List<KeyCode> { KeyCode.RightAlt, KeyCode.RightControl, KeyCode.RightShift, KeyCode.Keypad0, KeyCode.Return };
        }
        else
        {
            upKey = KeyCode.W;
            downKey = KeyCode.S;
            leftKey = KeyCode.A;
            rightKey = KeyCode.D;
            ultKey = new List<KeyCode> { KeyCode.Space, KeyCode.LeftControl, KeyCode.LeftShift, KeyCode.LeftAlt };
        }
        ball = GameObject.FindGameObjectWithTag("Ball");
        Init();
        InitOpponent();
    }

    protected bool UpKey() { return Input.GetKey(upKey); }
    protected bool UpKeyUp() { return Input.GetKeyUp(upKey); }
    protected bool UpKeyDown() { return Input.GetKeyDown(upKey); }
    protected bool DownKey() { return Input.GetKey(downKey); }
    protected bool DownKeyDown() { return Input.GetKeyDown(downKey); }
    protected bool LeftKey() { return Input.GetKey(leftKey); }
    protected bool RightKey() { return Input.GetKey(rightKey); }
    protected bool UltKeyDown()
    {
        foreach (KeyCode key in ultKey)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }
}
