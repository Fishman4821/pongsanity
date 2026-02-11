using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PaddleType;

public class CharacterData : MonoBehaviour
{
    public Character playerCharacter;
    public Character opponentCharacter;
    public bool twoPlayer;
    public int musicTrack = 0;
    public List<AudioClip> musicTracks;
}
