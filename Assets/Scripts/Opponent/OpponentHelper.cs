using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace OpponentHelper
{
    public class Self
    {
        public MonoBehaviour mb;
        public GameObject self;
        public Rigidbody2D rb;
        public GameObject ballObj;
        public Rigidbody2D ball;
        public float speed;
        public float paddleHeight;

        public float X { get { return self.transform.position.x; } }
        public float Y { get { return self.transform.position.y; } }

        public Self(GameObject self_, GameObject ballObj_, float speed_, float paddleHeight_)
        {
            self = self_;
            rb = self.GetComponent<Rigidbody2D>();
            ballObj = ballObj_;
            ball = ballObj.GetComponent<Rigidbody2D>();
            speed = speed_;
            mb = self.GetComponent<MonoBehaviour>();
            paddleHeight = paddleHeight_;
        }

        public Self(GameObject self_, Rigidbody2D rb_, GameObject ballObj_, float speed_, float paddleHeight_)
        {
            self = self_;
            rb = rb_;
            ballObj = ballObj_;
            ball = ballObj.GetComponent<Rigidbody2D>();
            speed = speed_;
            mb = self.GetComponentInParent<MonoBehaviour>();
            paddleHeight = paddleHeight_;
        }

        public void MoveUp()
        {
            Vector2 velocity = rb.velocity;
            velocity.y = speed;
            rb.velocity = velocity;
        }

        public void MoveDown()
        {
            Vector2 velocity = rb.velocity;
            velocity.y = -speed;
            rb.velocity = velocity;
        }

        public void Stop()
        {
            Vector2 velocity = rb.velocity;
            velocity.y = 0;
            rb.velocity = velocity;
        }

        public void ClampYPosition(float min, float max)
        {
            Vector3 pos = self.transform.position;
            if (pos.y > max)
            {
                pos.y = max;
            }
            else if (pos.y < min)
            {
                pos.y = min;
            }
            self.transform.position = pos;
        }

        public Vector2 BallPosition()
        {
            return ball.transform.position;
        }

        public float ballX { get { return ball.transform.position.x; } }
        public float ballY { get { return ball.transform.position.y; } }

        public Vector2 BallVelocity()
        {
            return ball.GetComponent<Rigidbody2D>().velocity;
        }

        public bool BallSide()
        {
            return BallPosition().x > 0;
        }

        public bool ballMovingRight()
        {
            return BallVelocity().x > 0;
        }

        private Vector2 previousBallVelocity = Vector2.zero;
        public bool ballVelChanged
        {
            get
            {
                Vector2 temp = previousBallVelocity;
                previousBallVelocity = BallVelocity();
                return temp != previousBallVelocity;
            }
        }

        public float BallPredictedY(float targetX, float defaultY = 0f)
        {
            if (BallVelocity().x <= 0) return defaultY;

            const int steps = 1000;
            const float dt = 0.01f;
            float top = 3f - 0.125f / 2f;
            float bottom = -3f + 0.125f / 2f;
            float prevX;
            float prevY;
            float x = ballX;
            float y = ballY;
            float vx = BallVelocity().x;
            float vy = BallVelocity().y;
            for (int i = 0; i < steps; i++)
            {
                prevX = x;
                prevY = y;
                x += vx * dt;
                y += vy * dt;
                if (y > top)
                {
                    y = top + top - y;
                    vy *= -1;
                }
                else if (y < bottom)
                {
                    y = bottom + bottom - y;
                    vy *= -1;
                }

                // Debug.DrawLine(new Vector3(prevX, prevY, -2), new Vector3(x, y, -2), Color.red, 0f, false);
                if (prevX - targetX <= 0 && x - targetX >= 0)
                {
                    return y;
                }
            }
            return defaultY;
        }

        private IEnumerator WaitCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        public void Wait(float seconds)
        {
            mb.StartCoroutine(WaitCoroutine(seconds));
        }
    }
}