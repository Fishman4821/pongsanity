using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaddleType;
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

    public void LoadMusic(CharacterData characterData)
    {
        characterData.musicTracks = new List<AudioClip>();
        for (int i = 1; i <= 5; i++)
        {
            characterData.musicTracks.Add(Resources.Load<AudioClip>($"Music/Song {i}"));
        }
    }

    public void Confirm()
    {
        GameObject data = GameObject.Find("CharacterData");
        if (data == null)
        {
            data = new GameObject("CharacterData");
            DontDestroyOnLoad(data);
        }
        if (!data.TryGetComponent(out CharacterData characterData))
        {
            characterData = data.AddComponent<CharacterData>();
            LoadMusic(characterData);
        }
        characterData.playerCharacter = playerCharacter;
        characterData.opponentCharacter = opponentCharacter;
        characterData.twoPlayer = twoPlayer;
        characterData.musicTrack = Random.Range(0, characterData.musicTracks.Count);
        LoadNextInBuild();
    }
}
