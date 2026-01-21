using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaddleType;

public class Player : MonoBehaviour
{
    public Character playerType;

    void Start()
    {
        playerType = GameObject.Find("CharacterSelector").GetComponent<CharacterSelector>().playerCharacter;
        switch (playerType)
        {
            case Character.Paddle:
                Paddle p = gameObject.AddComponent<Paddle>();
                break;
            case Character.Double:
                Double d = gameObject.AddComponent<Double>();
                break;
            case Character.Three:
                Three t = gameObject.AddComponent<Three>();
                break;
            case Character.Launcher:
                Launcher l = gameObject.AddComponent<Launcher>();
                break;
        }
    }
}
