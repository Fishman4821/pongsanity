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
        self = new Self(topObj, topRb, ball, speed, 1f);
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
        float predictedY = self.BallPredictedY(3.875f, float.NaN);

        // Debug.DrawLine(new Vector3(3.875f, !float.IsNaN(predictedY) ? predictedY : -10, -2), new Vector3(4.875f, !float.IsNaN(predictedY) ? predictedY : -10, -2), Color.yellow, 0.5f, false);

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
                if (predictedY > -0.3f)
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

        
        // Debug.DrawLine(new Vector3(self.X, self.Y + self.paddleHeight / 2f, -2), new Vector3(self.X + 1f, self.Y + self.paddleHeight / 2f, -2), Color.green, 0f, false);
        // Debug.DrawLine(new Vector3(self.X, self.Y - self.paddleHeight / 2f, -2), new Vector3(self.X + 1f, self.Y - self.paddleHeight / 2f, -2), Color.blue, 0f, false);
        // Debug.DrawLine(new Vector3(self.X, -self.Y + self.paddleHeight / 2f, -2), new Vector3(self.X + 1f, -self.Y + self.paddleHeight / 2f, -2), Color.magenta, 0f, false);
        // Debug.DrawLine(new Vector3(self.X, -self.Y - self.paddleHeight / 2f, -2), new Vector3(self.X + 1f, -self.Y - self.paddleHeight / 2f, -2), Color.red, 0f, false);

        // Debug.DrawLine(new Vector3(4f, predictedY < -0.3f ? -self.Y : self.Y, -2), new Vector3(3f, predictedY < -0.3f ? -self.Y + topRb.velocity.y / 14.0f : self.Y + topRb.velocity.y / 74.0f, -2), Color.cyan, 0, false);
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

        // Debug.Log("Predicted Y: " + predictedY.ToString("F2") + " Paddle Y: " + self.Y.ToString("F2") + " Vel Y: " + self.rb.velocity.y.ToString("F2") + " Ball Vel X: " + self.ballX.ToString("F2"));

        ClampYPosition();
        UpdateUlt();
        if (ball.GetComponent<Ball>().scoreValue == 1 && UltActive)
        {
            ResetUlt();
        }
    }
}
