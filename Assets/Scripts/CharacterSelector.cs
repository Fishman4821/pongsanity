using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaddleType;
using System;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

using static NextScene;

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

    public void confirm()
    {
        GameObject data = new GameObject("CharacterData");
        CharacterData characterData = data.AddComponent<CharacterData>();
        characterData.playerCharacter = playerCharacter;
        characterData.opponentCharacter = opponentCharacter;
        DontDestroyOnLoad(data);
        LoadNextInBuild();
    }
}
