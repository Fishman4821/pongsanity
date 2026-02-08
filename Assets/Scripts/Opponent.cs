using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaddleType;

public class Opponent : MonoBehaviour
{
    public UltMeter meter;
    public Character opponentType;

    void Start()
    {
        GameObject cs = GameObject.Find("CharacterData");
        if (cs != null)
        {
            opponentType = cs.GetComponent<CharacterData>().opponentCharacter;
        }
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
        meter.Init(gameObject, opponentType);
    }
}
