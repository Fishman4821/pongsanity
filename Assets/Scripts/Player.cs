using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PaddleType
    {
        Paddle,
        Double,
        Three,
        Launcher
    }
    public PaddleType playerType;

    void Start()
    {
        switch (playerType)
        {
            case PaddleType.Paddle:
                Paddle p = gameObject.AddComponent<Paddle>();
                p.Opponent = false;
                break;
            case PaddleType.Double:
                Double d = gameObject.AddComponent<Double>();
                d.Opponent = false;
                break;
            case PaddleType.Three:
                Three t = gameObject.AddComponent<Three>();
                t.Opponent = false;
                break;
            case PaddleType.Launcher:
                Launcher l = gameObject.AddComponent<Launcher>();
                l.Opponent = false;
                break;
        }
    }

    void Update()
    {
        
    }
}
