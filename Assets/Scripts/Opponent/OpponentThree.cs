using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using OpponentHelper;

public class OpponentThree : Three
{
    public float reactionTime = 0.07f;
    public float reactionVariance = 0.02f;
    private bool reacting = false;

    public override bool Opponent { get; set; } = true;

    private Self self;

    protected override void InitOpponent()
    {
        self = new Self(gameObject, ball, speed, 2f);
    }

    private float waitTime = 0f;
    private float timeWaited = 0f;

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
                if (!reacting)
                {
                    if (waitTime == 0f) {
                        waitTime = reactionTime + Random.Range(-reactionVariance, reactionVariance);
                    }
                    timeWaited += Time.deltaTime;
                    if (timeWaited >= waitTime)
                    {
                        waitTime = 0f;
                        timeWaited = 0f;
                        reacting = true;
                    } else
                    {
                        break;
                    }
                }
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
