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

    public void basicPaddleSet()
    {
        playerType = PaddleType.Paddle;
    }

    public void doublePaddleSet()
    {
        playerType = PaddleType.Double;
    }

    public void triPaddleSet()
    {
        playerType = PaddleType.Three;
    }

    public void launcherSet()
    {
        playerType = PaddleType.Launcher;
    }

    void Start()
    {
        switch (playerType)
        {
            case PaddleType.Paddle:
                Paddle p = gameObject.AddComponent<Paddle>();
                break;
            case PaddleType.Double:
                Double d = gameObject.AddComponent<Double>();
                break;
            case PaddleType.Three:
                Three t = gameObject.AddComponent<Three>();
                break;
            case PaddleType.Launcher:
                Launcher l = gameObject.AddComponent<Launcher>();
                break;
        }
    }
}
