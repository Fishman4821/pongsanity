using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpponentHelper;

public class OpponentDouble : Double
{
    public float reactionTime = 0.1f;
    public float reactionVariance = 0.05f;
    public float overshootTime = 0.06f;
    public float overshootVariance = 0.01f;
    public float overshootBias = 0.85f;

    public override bool Opponent { get; set; } = true;

    private Self self;

    protected override void InitOpponent()
    {
        self = new Self(topObj, topRb, ball, speed, 0.375f);
    }

    private float waitTime = 0f;
    private float timeWaited = 0f;
    private float ultWaitTime = 0f;
    private float ultTimeWaited = 0f;

    public enum State
    {
        Idle,
        Reacting,
        Moving,
        Stopping,
    }

    public State state = State.Idle;

    void Update()
    {
        float predictedY = self.BallPredictedY(3.9375f, float.NaN);

        switch (state)
        {
            case State.Idle:
                if (self.BallSide() && !float.IsNaN(predictedY))
                {
                    state = State.Reacting;
                }
                break;
            case State.Reacting:
                if (waitTime == 0f)
                {
                    waitTime = reactionTime + Random.Range(-reactionVariance, reactionVariance);
                }
                timeWaited += Time.deltaTime;
                if (timeWaited >= waitTime)
                {
                    waitTime = 0f;
                    timeWaited = 0f;
                    state = State.Moving;
                }
                break;
            case State.Moving:
                if (predictedY > -0.15f)
                {
                    if (predictedY > self.Y + self.paddleHeight / 2f)
                    {
                        self.MoveUp();
                    }
                    else if (predictedY < self.Y - self.paddleHeight / 2f)
                    {
                        self.MoveDown();
                    }
                    else
                    {
                        state = State.Stopping;
                    }
                }
                else
                {
                    if (predictedY > -self.Y + self.paddleHeight / 2f)
                    {
                        self.MoveDown();
                    }
                    else if (predictedY < -self.Y - self.paddleHeight / 2f)
                    {
                        self.MoveUp();
                    }
                    else
                    {
                        state = State.Stopping;
                    }
                }
                break;
            case State.Stopping:
                if (waitTime == 0f)
                {
                    waitTime = overshootTime + Random.Range(-overshootVariance, overshootVariance) + overshootBias * overshootVariance;
                }
                timeWaited += Time.deltaTime;
                if (timeWaited >= waitTime)
                {
                    waitTime = 0f;
                    timeWaited = 0f;
                    self.Stop();
                    state = State.Idle;
                }
                break;
        }

        if (UltReady() && self.ballX < -3f)
        {
            if (ultWaitTime == 0f)
            {
                ultWaitTime = Random.Range(0f, reactionTime);
            }
            ultTimeWaited += Time.deltaTime;
            if (ultTimeWaited >= ultWaitTime)
            {
                ultWaitTime = 0f;
                ultTimeWaited = 0f;
                Ult();
            }
        }

        ClampYPosition();
        UpdateUlt();
        if (ball.GetComponent<Ball>().scoreValue == 1 && UltActive)
        {
            ResetUlt();
        }
    }
}
