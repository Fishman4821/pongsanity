using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
    public GameObject ball = null;
    protected virtual bool Opponent { get; set; } = false;

    public float UltMeter = 0f;
    public float UltMeterMax = 1f;
    public virtual float UltDuration { get; set; }
    public float UltTimer = 0f;
    public bool UltActive = false;
    public abstract void Ult();
    public abstract void ResetUlt();

    protected abstract void Init();

    protected virtual void InitOpponent() {}

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

        if (!Opponent && Input.GetKeyDown(KeyCode.Space) && UltReady())
        {
            Ult();
        }
    }

    protected bool Waited = false;

    protected IEnumerator Wait(float s)
    {
        yield return new WaitForSecondsRealtime(s);
        Waited = true;
    }

    public void IncrementUltMeter()
    {
        if (!UltActive) UltMeter = Mathf.Clamp(UltMeter + 0.25f, 0f, UltMeterMax);
    }

    public bool UltReady()
    {
        return UltMeter >= UltMeterMax && !UltActive;
    }

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        Init();
        InitOpponent();
    }
}
