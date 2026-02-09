using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaddleType;

public class Opponent : MonoBehaviour
{
    public UltMeter meter;
    public Character opponentType;
    public bool twoPlayer = false;

    void Start()
    {
        GameObject cs = GameObject.Find("CharacterData");
        if (cs != null)
        {
            opponentType = cs.GetComponent<CharacterData>().opponentCharacter;
            twoPlayer = cs.GetComponent<CharacterData>().twoPlayer;
        }
        if (!twoPlayer)
        {
            switch (opponentType)
            {
                case Character.Paddle:
                    OpponentPaddle p = gameObject.AddComponent<OpponentPaddle>();
                    break;
                case Character.Double:
                    OpponentDouble d = gameObject.AddComponent<OpponentDouble>();
                    break;
                case Character.Three:
                    OpponentThree t = gameObject.AddComponent<OpponentThree>();
                    break;
                case Character.Launcher:
                    OpponentLauncher l = gameObject.AddComponent<OpponentLauncher>();
                    break;
            }
        }
        else
        {
            switch (opponentType)
            {
                case Character.Paddle:
                    Paddle p = gameObject.AddComponent<Paddle>();
                    p.Opponent = true;
                    p.twoPlayer = true;
                    break;
                case Character.Double:
                    Double d = gameObject.AddComponent<Double>();
                    d.Opponent = true;
                    d.twoPlayer = true;
                    break;
                case Character.Three:
                    Three t = gameObject.AddComponent<Three>();
                    t.Opponent = true;
                    t.twoPlayer = true;
                    break;
                case Character.Launcher:
                    Launcher l = gameObject.AddComponent<Launcher>();
                    l.Opponent = true;
                    l.twoPlayer = true;
                    break;
            }
        }
        meter.Init(gameObject, opponentType);
    }
}
