using UnityEngine;

namespace ProjectBubble.Core.Combat
{
    public abstract class Projectile : MonoBehaviour
    {
        private Rigidbody2D _body;
        private Vector2 _lastPosition;
        private float _traveledDistance;
        [SerializeField] protected float _movementSpeed;
        [SerializeField] private float _range;
        [SerializeField] private float _damage;
        public Vector2 Right { get; set; }
        protected Rigidbody2D Body
        {
            get
            {
                if (_body == null)
                    _body = GetComponent<Rigidbody2D>();
                return _body;
            }
        }

        protected virtual void Start()
        {
            _lastPosition = transform.position;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject gameObject = collision.gameObject;
            OnCollision(gameObject);
            TryApplyDamage(gameObject);
        }

        protected void CalculateTraveledDistance()
        {
            float distance = Vector2.Distance(_lastPosition, transform.position);
            _traveledDistance += distance;
            _lastPosition = transform.position;
        }

        protected bool HasSurpassedRange()
        {
            return _traveledDistance >= _range;
        }

        protected bool TryApplyDamage(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
                return true;
            }

            return false;
        }

        protected virtual void OnCollision(GameObject gameObject) { }
    }
}
