using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToPaddlePass : MonoBehaviour
{
    public static void LoadPaddlePass()
    {
        SceneManager.LoadScene("PaddlePass");
    }
}
