using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public int playerScore = 0;
    public int opponentScore = 0;
    public TMPro.TextMeshProUGUI playerScoreText;
    public TMPro.TextMeshProUGUI opponentScoreText;

    void Start()
    {
        playerScoreText.text = playerScore.ToString();
        opponentScoreText.text = opponentScore.ToString();
    }

    void Update()
    {
        
    }

    public void IncrementPlayerScore(bool doublePoints)
    {
        playerScore += doublePoints ? 2 : 1;
        playerScoreText.text = playerScore.ToString();
    }

    public void IncrementOpponentScore(bool doublePoints)
    {
        opponentScore += doublePoints ? 2 : 1;
        opponentScoreText.text = opponentScore.ToString();
    }
}
