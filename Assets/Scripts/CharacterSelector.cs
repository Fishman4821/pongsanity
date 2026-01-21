using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaddleType;
using System;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public Character playerCharacter;
    public Character opponentCharacter;

    public void setPlayer(int c)
    {
        playerCharacter = c switch
        {
            0 => Character.Paddle,
            1 => Character.Double,
            2 => Character.Three,
            3 => Character.Launcher,
            _ => Character.Paddle
        };
    }

    public void setOpponent(int c)
    {
        opponentCharacter = c switch
        {
            0 => Character.Paddle,
            1 => Character.Double,
            2 => Character.Three,
            3 => Character.Launcher,
            _ => Character.Paddle
        };
    }

    void Awake()
    {
        int numCharacterSelectors = FindObjectsOfType<CharacterSelector>().Length;
        if (numCharacterSelectors != 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
