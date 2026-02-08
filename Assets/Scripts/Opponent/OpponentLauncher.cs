using OpponentHelper;
using UnityEngine;

public class OpponentLauncher : Launcher
{
    public float reactionTime = 0.05f;
    public float reactionVariance = 0.01f;
    public float overshootTime = 0.05f;
    public float overshootVariance = 0.01f;
    public float overshootBias = 0.65f;
    public float launchTime = 2.0f;
    public float launchVariance = 1.75f;
    public float launchBias = -0.25f;
    public float ultChance = 0.05f;
    protected override bool Opponent { get; set; } = true;
    private Self self;
    private GameObject player;

    protected override void InitOpponent()
    {
        self = new Self(gameObject, ball, speed, 0.75f / 2f);
        Player[] players = FindObjectsOfType<Player>();
        if (players.Length > 0)
        {
            player = players[0].gameObject;
        }
    }

    public enum State
    {
        Idle,
        Reacting,
        Moving,
        Stopping,
        Rotating,
        Launching,
        Resetting,
    }

    public State state = State.Idle;

    private float predictedY = 0f;

    private float waitTime = 0f;
    private float timeWaited = 0f;

    protected float TargetAngleRand()
    {
        float r = Random.value;
        float v = Mathf.Lerp(0.3f, 1.3f, r * r);
        return 1 - v;
    }

    private float targetAngle = 0f;
    private bool targetSet = false;

    private float a1, a2;
    protected void SetTargetAngle()
    {
        if (player.transform.position.y > 0)
        {
            a1 = Mathf.Atan(Mathf.Abs(transform.position.y + 3f) / 8f) * Mathf.Rad2Deg;
        }
        else
        {
            a1 = -Mathf.Atan(Mathf.Abs(transform.position.y - 3f) / 8f) * Mathf.Rad2Deg;
        }
        if (player.transform.position.y > transform.position.y)
        {
            a2 = -Mathf.Atan(Mathf.Abs(transform.position.y - player.transform.position.y) / 8f) * Mathf.Rad2Deg;
        }
        else
        {
            a2 = Mathf.Atan(Mathf.Abs(transform.position.y - player.transform.position.y) / 8f) * Mathf.Rad2Deg;
        }
        a1 += 180f;
        a2 += 180f;
        targetAngle = Mathf.MoveTowardsAngle(a1, a2, TargetAngleRand() * Mathf.Abs(Mathf.DeltaAngle(a1, a2)));
        targetSet = true;
    }

    void Update()
    {
        predictedY = self.BallPredictedY(3.875f, float.NaN);

        switch (state)
        {
            case State.Idle:
                if (self.BallSide() && !float.IsNaN(predictedY) && !holdingBall)
                {
                    state = State.Reacting;
                }
                if (holdingBall)
                {
                    state = State.Rotating;
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
            case State.Rotating:
                if (!targetSet)
                {
                    SetTargetAngle();
                }
                if (Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetAngle + 180f)) < 3f)
                {
                    //rb.angularVelocity = 0f;
                    transform.rotation = Quaternion.Euler(0f, 0f, targetAngle + 180f);
                    state = State.Launching;
                }
                else
                {
                    //rb.angularVelocity = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, targetAngle, 1) - transform.rotation.eulerAngles.z < 0 ? turnSpeed : -turnSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, targetAngle + 180f, turnSpeed * Time.deltaTime));
                }
                break;
            case State.Launching:
                if (UltReady() && Random.value < ultChance)
                {
                    Ult();
                }
                if (waitTime == 0f)
                {
                    waitTime = launchTime + Random.Range(-launchVariance, launchVariance) + launchBias * launchVariance;
                }
                timeWaited += Time.deltaTime;
                if (timeWaited >= waitTime)
                {
                    waitTime = 0f;
                    timeWaited = 0f;
                    Launch(180f);
                    targetAngle = 0f;
                    targetSet = false;
                    state = State.Idle;
                }
                break;
            case State.Resetting:
                if (waitTime == 0f)
                {
                    waitTime = reactionTime + Random.Range(-reactionVariance, reactionVariance);
                }
                timeWaited += Time.deltaTime;
                if (timeWaited >= waitTime)
                {
                    waitTime = 0f;
                    timeWaited = 0f;
                    state = State.Idle;
                }
                break;
        }

        if (!holdingBall && state != State.Resetting)
        {
            ResetRotation();
        } else
        {
            ball.transform.position = transform.position;
        }

        self.ClampYPosition(-2.625f, 2.625f);
        UpdateUlt();
    }
}