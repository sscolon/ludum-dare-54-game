using ProjectBubble.Core.Combat;
using UnityEngine;

namespace ProjectBubble.Content.Enemies
{
    public class EnemyBasic : Entity
    {
        private Rigidbody2D _rb2D;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;
        private void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb2D.Move(Vector2.zero, _acceleration, _deceleration);
        }
    }
}
