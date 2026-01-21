using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using OpponentHelper;

public class OpponentThree : Three
{
    public float reactionTime = 0.1f;
    public float reactionVariance = 0.05f;
    private bool reacting = false;

    protected override bool Opponent { get; set; } = true;

    private Self self;

    protected override void InitOpponent()
    {
        self = new Self(gameObject, ball, speed, 2f);
    }

    void Update()
    {
        float predictedY = self.BallPredictedY(3.875f, float.NaN);
        if (self.ballNotMoving() && self.ballX > -1f)
        {
            if (pos < 0)
            {
                MoveUp();
            }
            else if (pos > 0)
            {
                MoveDown();
            }
        }
        else if (self.ballX > -2f && !float.IsNaN(predictedY))
        {
            do
            {
                if (!reacting) StartCoroutine(Wait(reactionTime + Random.Range(-reactionVariance, reactionVariance)));
                if (!reacting && Waited)
                {
                    Waited = false;
                    break;
                }
                reacting = true;
                if (UltReady() && (predictedY > 1f && pos == -1 || predictedY < -1f && pos == 1))
                {
                    Ult();
                }
                if (predictedY > self.Y + self.paddleHeight / 2f && reacting)
                {
                    MoveUp();
                }
                else if (predictedY < self.Y - self.paddleHeight / 2f && reacting)
                {
                    MoveDown();
                } 
                else
                {
                    reacting = false;
                }
            } while (false);
        }
        UpdatePos();
        UpdateUlt();
    }
}
