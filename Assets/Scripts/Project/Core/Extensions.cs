using UnityEngine;

namespace ProjectBubble
{
    public static class Extensions
    {
        public static void Move(this Rigidbody2D rb2D, Vector2 targetSpeed, float acceleration, float deceleration)
        {
            Vector2 velocity = rb2D.velocity;
            Vector2 diff = targetSpeed - velocity;
            float accelX = (Mathf.Abs(targetSpeed.x) > 0.01f) ? acceleration : deceleration;
            float accelY = (Mathf.Abs(targetSpeed.y) > 0.01f) ? acceleration : deceleration;

            Vector2 movement = Vector2.zero;
            movement.x = Mathf.Abs(diff.x) * accelX * Mathf.Sign(diff.x) * acceleration;
            movement.y = Mathf.Abs(diff.y) * accelY * Mathf.Sign(diff.y) * acceleration;
            rb2D.AddForce(movement);
        }
    }
}
