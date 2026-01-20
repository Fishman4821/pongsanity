using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpponentHelper;
public class OpponentPaddle : Paddle
{
    public float reactionTime = 0.2f;

    private bool reacting = false;

    protected override bool Opponent { get; set; } = true;

    private Self self;

    protected override void InitOpponent()
    {
        self = new Self(gameObject, ball, speed, 1f);
    }

    void Update()
    {
        float predictedY = self.BallPredictedY(3.875f, float.NaN);
    
        // Debug.DrawLine(new Vector3(self.X, self.Y + self.paddleHeight / 2f - 0.1f, -2), new Vector3(self.X + 1f, self.Y + self.paddleHeight / 2f - 0.1f, -2), Color.green, 0f, false);
        // Debug.DrawLine(new Vector3(self.X, self.Y - self.paddleHeight / 2f + 0.1f, -2), new Vector3(self.X + 1f, self.Y - self.paddleHeight / 2f + 0.1f, -2), Color.blue, 0f, false);
        // Debug.DrawLine(new Vector3(3.875f, !float.IsNaN(predictedY) ? predictedY : -10, -2), new Vector3(4.875f, !float.IsNaN(predictedY) ? predictedY : -10, -2), Color.yellow, 0.5f, false);
        
        if (self.BallSide() && !float.IsNaN(predictedY))
        {
            if (!reacting) self.Wait(reactionTime);
            reacting = true;
            if (predictedY > self.Y + self.paddleHeight / 2f - 0.1f && reacting)
            {
                self.MoveUp();
            }
            else if (predictedY < self.Y - self.paddleHeight / 2f + 0.1f && reacting)
            {
                self.MoveDown();
            }
            else
            {
                self.Stop();
                reacting = false;
            }
        }

        // Debug.Log("Predicted Y: " + predictedY.ToString("F2") + " Paddle Y: " + self.Y.ToString("F2") + " Vel Y: " + self.rb.velocity.y.ToString("F2") + " Ball Vel X: " + self.ballX.ToString("F2"));

        self.ClampYPosition(-2.5f, 2.5f);
    }
}
