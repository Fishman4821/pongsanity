using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaddleType;
using System;
//using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

using static NextScene;

public class CharacterSelector : MonoBehaviour
{
    public Character playerCharacter;
    public Character opponentCharacter;
    public bool twoPlayer;

    public void SetPlayer(int c)
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

    public void SetOpponent(int c)
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

    public void SetTwoPlayer(bool twoPlayer_)
    {
        twoPlayer = twoPlayer_;
    }

    public void Confirm()
    {
        GameObject data = new GameObject("CharacterData");
        CharacterData characterData = data.AddComponent<CharacterData>();
        characterData.playerCharacter = playerCharacter;
        characterData.opponentCharacter = opponentCharacter;
        characterData.twoPlayer = twoPlayer;
        DontDestroyOnLoad(data);
        LoadNextInBuild();
    }
}
