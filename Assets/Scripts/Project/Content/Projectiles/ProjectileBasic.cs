using DDCore;
using ProjectBubble.Core.Combat;
using UnityEngine;

namespace ProjectBubble.Content.Projectiles
{
    public class ProjectileBasic : Projectile
    {
        private void Update()
        {
            CalculateTraveledDistance();
            if (HasSurpassedRange())
            {
                Death();
            }
        }

        private void FixedUpdate()
        {
            Body.velocity = Right * _movementSpeed;
        }

        protected override void OnCollision(GameObject gameObject)
        {
            base.OnCollision(gameObject);
            Death();
        }

        private void Death()
        {
            //We can put VFX here later.
            Destroy(gameObject);
        }
    }
}
