using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaddleType;

public class Opponent : MonoBehaviour
{
    public Character opponentType;

    void Start()
    {
        opponentType = GameObject.Find("CharacterSelector").GetComponent<CharacterSelector>().opponentCharacter;
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
}
