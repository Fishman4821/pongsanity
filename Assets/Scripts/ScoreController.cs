using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool overtime = false;
    private bool twoPlayer = false;
    private Ball ball;
    public float timer = 180f;
    private float countDown = 4f;

    void EndGame()
    {
        if (playerScore > opponentScore)
        {
            if (twoPlayer)
            {
                SceneManager.LoadScene("PlayerOneWin");
            }
            else
            {
                SceneManager.LoadScene("PlayerWin");
            }
        }
        else if (playerScore < opponentScore)
        {
            if (twoPlayer)
            {
                SceneManager.LoadScene("PlayerTwoWin");
            }
            else
            {
                SceneManager.LoadScene("ComputerWin");
            }
        }
        else
        {
            // SceneManager.LoadScene("Tiebreaker");
            overtime = true;
        }
    }

    void Start()
    {
        twoPlayer = GameObject.Find("CharacterData").GetComponent<CharacterData>().twoPlayer;
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
            if (overtime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer -= Time.deltaTime;
            }

            timerText.text = (overtime ? "+" : "") + (int)Mathf.Max(timer / 60f, 0) + ":" + ((int)(Mathf.Max(timer, 0) % 60f)).ToString("00");

            if (timer <= 0f)
            {
                EndGame();
            }
        }
        else
        {
            countDown -= Time.deltaTime;

            if (Mathf.Floor(countDown) > 0)
            {
                countDownText.text = Mathf.Floor(countDown).ToString();
                audioSource.Play();
            }
            else
            {
                countDownText.text = "Go!";
            }
            if (countDown <= 0f)
            {
                countDownText.gameObject.SetActive(false);
                ball.FirstServe();
            }
        }
    }

    public void IncrementPlayerScore(int scoreValue)
    {
        playerScore += scoreValue;
        playerScoreText.text = playerScore.ToString();
        if (overtime)
        {
            EndGame();
        }
    }

    public void IncrementOpponentScore(int scoreValue)
    {
        opponentScore += scoreValue;
        opponentScoreText.text = opponentScore.ToString();
        if (overtime)
        {
            EndGame();
        }
    }
}
