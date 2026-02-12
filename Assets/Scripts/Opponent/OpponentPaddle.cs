using UnityEngine;
using OpponentHelper;

public class OpponentPaddle : Paddle
{
    public float reactionTime = 0.05f;
    public float reactionVariance = 0.1f;
    public float overshootTime = 0.05f;
    public float overshootVariance = 0.01f;
    public float overshootBias = 0.65f;
    public float ultChance = 0.01f;

    public override bool Opponent { get; set; } = true;

    private Self self;

    private float predictedY = 0f;

    private enum State
    {
        Idle,
        Reacting,
        Moving,
        Stopping,
    }

    private State state = State.Idle;

    protected override void InitOpponent()
    {
        self = new Self(gameObject, ball, speed, 1f);
    }

    float waitTime = 0f;
    float timeWaited = 0f;

    void Update()
    {
        predictedY = self.BallPredictedY(3.875f, float.NaN);

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

        if (UltReady() && self.ballMovingRight() && self.ballX > -2.5f && self.ballX < 1f && Random.value < ultChance)
        {
            Ult();
        }

        self.ClampYPosition(-2.5f, 2.5f);
        UpdateUlt();
    }
}
