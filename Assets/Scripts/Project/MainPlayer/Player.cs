using UnityEngine;

namespace ProjectBubble.MainPlayer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        private Rigidbody2D _rb2D;
        private float _inputX;
        private float _inputY;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;
        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _inputX = Input.GetAxisRaw("Horizontal");
            _inputY = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            Vector2 input = new Vector2(_inputX, _inputY);
            Vector2 normalizedInput = input.normalized;
            Vector2 targetSpeed = normalizedInput * _movementSpeed;
            Move(targetSpeed, _acceleration, _deceleration);
        }

        private void Move(Vector2 targetSpeed, float acceleration, float deceleration)
        {
            Vector2 velocity = _rb2D.velocity;
            Vector2 diff = targetSpeed - velocity;
            float accelX = (Mathf.Abs(targetSpeed.x) > 0.01f) ? acceleration : deceleration;
            float accelY = (Mathf.Abs(targetSpeed.y) > 0.01f) ? acceleration : deceleration;

            Vector2 movement = Vector2.zero;
            movement.x = Mathf.Abs(diff.x) * accelX * Mathf.Sign(diff.x) * acceleration;
            movement.y = Mathf.Abs(diff.y) * accelY * Mathf.Sign(diff.y) * acceleration;
            _rb2D.AddForce(movement);
        }
    }
}
