using ProjectBubble.Core;
using ProjectBubble.Core.Combat;
using UnityEngine;

namespace ProjectBubble.Content.Enemies
{
    public abstract class Enemy : Entity
    {
        //Loots
        [SerializeField] private Loot _loot;
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            VFXUtil.DoSpriteFlash(gameObject);
        }

        public override void Death()
        {
            base.Death();
            _loot?.Instantiate(transform.position);
            Destroy(gameObject);
        }
    }
}
