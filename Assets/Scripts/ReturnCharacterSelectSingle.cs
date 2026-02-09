using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnCharacterSelectSingle : MonoBehaviour
{
    public static void ReturnToCharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
}

