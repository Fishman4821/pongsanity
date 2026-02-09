using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public int playerScore = 0;
    public int opponentScore = 0;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI opponentScoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countDownText;
    public AudioSource audioSource;
    public bool started = false;
    private Ball ball;
    private float timer = 180f;
    private float countDown = 3f;

    void EndGame()
    {
        if (playerScore > opponentScore)
        {
            
        }
        else if (playerScore < opponentScore)
        {
            
        }
        else
        {
            
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        playerScoreText.text = playerScore.ToString();
        opponentScoreText.text = opponentScore.ToString();
        timerText.text = (int)Mathf.Max(timer / 60f, 0) + ":" + ((int)(Mathf.Max(timer, 0) % 60f)).ToString("00");
    }

    void Update()
    {
        if (started)
        {   
            timer -= Time.deltaTime;

            timerText.text = (int)Mathf.Max(timer / 60f, 0) + ":" + ((int)(Mathf.Max(timer, 0) % 60f)).ToString("00");

            if (timer <= 0f)
            {
                EndGame();
            }
        } else
        {
            countDown -= Time.deltaTime;
            if (Mathf.Floor(countDown) > 0)
            {
                countDownText.text = Mathf.Floor(countDown).ToString();
            } else
            {
                countDownText.text = "Go!";
            }
            if (countDown <= 0f)
            {
                countDownText.gameObject.SetActive(false);
                audioSource.Play();
                ball.FirstServe();
            }
        }
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
