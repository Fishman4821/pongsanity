using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpponentHelper;

public class OpponentLauncher : Launcher
{
    public float reactionTime = 0.3f;
    public float reactionVariance = 0.2f;
    public float overshootTime = 0.15f;
    public float overshootVariance = 0.1f;
    public float overshootBias = 0.65f;
    public float launchTime = 2.0f;
    public float launchVariance = 0.5f;
    public float launchBias = 0.65f;
    private bool reacting = false;
    private float targetAngle = 0f;
    private bool targetSet = false;
    private GameObject player;
    protected override bool Opponent { get; set; } = true;

    private Self self;

    protected override void InitOpponent()
    {
        self = new Self(gameObject, ball, speed, 0.75f / 2f);
        Player[] players = FindObjectsOfType<Player>();
        if (players.Length > 0)
        {
            player = players[0].gameObject;
        }
    }

    protected float TargetAngleRand()
    {
        float r = Random.value;
        float v = Mathf.Lerp(0.3f, 1.3f, r * r);
        return 1 - v;
    }

    float a1, a2;
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

    float FixAngle(float angle)
    {
        return angle + 180f;
    }

    void Update()
    {
        float predictedY = self.BallPredictedY(3.875f, float.NaN);

        // Debug.DrawLine(new Vector3(self.X, self.Y + self.paddleHeight / 2f - 0.1f, -2), new Vector3(self.X + 1f, self.Y + self.paddleHeight / 2f - 0.1f, -2), Color.green, 0f, false);
        // Debug.DrawLine(new Vector3(self.X, self.Y - self.paddleHeight / 2f + 0.1f, -2), new Vector3(self.X + 1f, self.Y - self.paddleHeight / 2f + 0.1f, -2), Color.blue, 0f, false);
        // Debug.DrawLine(new Vector3(3.875f, !float.IsNaN(predictedY) ? predictedY : -10, -2), new Vector3(4.875f, !float.IsNaN(predictedY) ? predictedY : -10, -2), Color.yellow, 0.5f, false);

        if (self.BallSide() && !float.IsNaN(predictedY) && !holdingBall)
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
                if (predictedY > self.Y + self.paddleHeight / 2f && reacting)
                {
                    self.MoveUp();
                }
                else if (predictedY < self.Y - self.paddleHeight / 2f && reacting)
                {
                    self.MoveDown();
                }
                else
                {
                    StartCoroutine(Wait(overshootTime + Random.Range(-overshootVariance, overshootVariance) + overshootBias * overshootVariance));
                    if (Waited)
                    {
                        Waited = false;
                        self.Stop();
                        reacting = false;
                    }
                }
            } while (false);
        }
        else if (holdingBall)
        {
            if (!targetSet)
            {
                StartCoroutine(Wait(reactionTime + Random.Range(-reactionVariance, reactionVariance)));
                if (Waited)
                {
                    Waited = false;
                    SetTargetAngle();
                }
            } else
            {
                if (Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.z, FixAngle(targetAngle))) < 5f)
                {
                    rb.angularVelocity = 0f;
                    StartCoroutine(Wait(launchTime + Random.Range(-launchVariance, launchVariance) + launchBias * launchVariance));
                    if (Waited)
                    {
                        Waited = false;
                        Launch();
                        targetSet = false;
                    }
                }
                else
                {
                    rb.angularVelocity = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, targetAngle, 1) - transform.rotation.eulerAngles.z < 0 ? turnSpeed : -turnSpeed;
                }
            }
        }

        if (!holdingBall)
        {
            ResetRotation();
        }

        if (targetSet)
        {
            //SetTargetAngle();
            Debug.Log("Target Angle: " + FixAngle(targetAngle).ToString("F2") + " Delta: " + Mathf.DeltaAngle(transform.rotation.eulerAngles.z, FixAngle(targetAngle)).ToString("F2"));
            Debug.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos((targetAngle) * Mathf.Deg2Rad) * 20, Mathf.Sin((targetAngle) * Mathf.Deg2Rad) * 20, 0f) * 2f, Color.cyan, 0f, false);
            Debug.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos((a1) * Mathf.Deg2Rad) * 20, Mathf.Sin((a1) * Mathf.Deg2Rad) * 20, 0f) * 2f, Color.green, 0f, false);
            Debug.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos((a2) * Mathf.Deg2Rad) * 20, Mathf.Sin((a2) * Mathf.Deg2Rad) * 20, 0f) * 2f, Color.red, 0f, false);
        }

        // Debug.Log("Predicted Y: " + predictedY.ToString("F2") + " Paddle Y: " + self.Y.ToString("F2") + " Vel Y: " + self.rb.velocity.y.ToString("F2") + " Ball Vel X: " + self.ballX.ToString("F2"));

        self.ClampYPosition(-2.625f, 2.625f);
        UpdateUlt();
    }
}
