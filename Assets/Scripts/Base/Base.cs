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

    void Awake()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }
}
