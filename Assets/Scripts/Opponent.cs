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
                p.Opponent = true;
                p.ball = GameObject.FindGameObjectWithTag("Ball");
                break;
            case PaddleType.Double:
                OpponentDouble d = gameObject.AddComponent<OpponentDouble>();
                d.Opponent = true;
                d.ball = GameObject.FindGameObjectWithTag("Ball");
                break;
            case PaddleType.Three:
                OpponentThree t = gameObject.AddComponent<OpponentThree>();
                t.Opponent = true;
                t.ball = GameObject.FindGameObjectWithTag("Ball");
                break;
            case PaddleType.Launcher:
                gameObject.AddComponent<Launcher>();
                break;
        }
    }

    void Update()
    {
        
    }
}
