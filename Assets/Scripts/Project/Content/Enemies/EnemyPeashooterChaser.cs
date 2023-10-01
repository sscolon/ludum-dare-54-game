using ProjectBubble.Content.Projectiles;
using ProjectBubble.Core;
using UnityEngine;

namespace ProjectBubble.Content.Enemies
{
    public class EnemyPeashooterChaser : Enemy
    {
        private Rigidbody2D _rb2D;
        private float _fireCountdown;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _movementDistance;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;

        [Header("Projectile Firing")]
        [SerializeField] private float _fireDelay;
        [SerializeField] private int _projectileCount;
        [SerializeField] private float _projectileSpread;
        [SerializeField] private float _projectileRandomSpread;
        [SerializeField] private ProjectileShootType _projectileShootType;

        [Header("Cannon Transform")]
        [SerializeField] private Transform _cannon;
        [SerializeField] private Transform _projectileSpawn;
        private void Start()
        {
            _fireCountdown = _fireDelay;
            _rb2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            SetOrientation();
        }

        private void FixedUpdate()
        {
            float distance = GameManager.DistanceToPlayer(transform.position);
            if(distance > _movementDistance)
            {
                Vector3 targetVelocity = GameManager.DirectionToPlayer(transform.position) * _movementDistance;
                _rb2D.Move(targetVelocity, _acceleration, _deceleration);
            }
        }

        private void SetOrientation()
        {
            //Rotate cannon towards the player and shoot projectiles on a timer.
            Quaternion rot = GameManager.RotationToPlayer(transform.position);
            _cannon.transform.rotation = rot;
            _fireCountdown -= Time.deltaTime;
            if (_fireCountdown <= 0)
            {
                Fire();
                _fireCountdown = _fireDelay;
            }
        }

        private void Fire()
        {
            switch (_projectileShootType)
            {
                case ProjectileShootType.Forward:
                    Vector3 direction = GameManager.DirectionToPlayer(transform.position);
                    ProjectileManager.CreateForwardProjectiles(_projectileSpawn.position, direction, _projectileCount, _projectileSpread, _projectileRandomSpread);
                    break;
                case ProjectileShootType.Circle:
                    ProjectileManager.CreateProjectileBurst(transform.position, _projectileCount, randomSpread: _projectileRandomSpread);
                    break;
            }
        }
    }
}
