using System;
using UnityEngine;

namespace ProjectBubble.Core.Combat
{
    public abstract class Entity : MonoBehaviour,
        IDamageable
    {
        private bool _isDead;
        [SerializeField] private float _health;
        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
                OnHealthValueChanged?.Invoke(_health);
            }
        }

        public event Action<float> OnHealthValueChanged;
        public event Action OnDamaged;
        public event Action OnDeath;
        public virtual void TakeDamage(float damage)
        {
            if (_isDead)
                return;
            Health -= damage;
            //We can put VFX and stuff here later.
            OnDamaged?.Invoke();
            if (Health <= 0)
            {
                Death();
                Health = 0;
            }
        }

        public virtual void Death()
        {
            _isDead = true;
            //We can put VFX and stuff here later.
            OnDeath?.Invoke();
        }
    }
}
