using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public enum PaddleType
    {
        Paddle,
        Double,
        Three,
        Launcher
    }
    public PaddleType opponentType;

    void Start()
    {
        switch (opponentType)
        {
            case PaddleType.Paddle:
                OpponentPaddle p = gameObject.AddComponent<OpponentPaddle>();
                break;
            case PaddleType.Double:
                OpponentDouble d = gameObject.AddComponent<OpponentDouble>();
                break;
            case PaddleType.Three:
                OpponentThree t = gameObject.AddComponent<OpponentThree>();
                break;
            case PaddleType.Launcher:
                OpponentLauncher l = gameObject.AddComponent<OpponentLauncher>();
                break;
        }
    }
}
