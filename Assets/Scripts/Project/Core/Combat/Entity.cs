using System;
using UnityEngine;

namespace ProjectBubble.Core.Combat
{
    public abstract class Entity : MonoBehaviour,
        IWaveBehaviour,
        IDamageable
    {
        private bool _isDead;

        [Header("Wave Spawn Attributes")]
        [SerializeField] private int _minSpawnWave;
        [SerializeField] private int _maxSpawnWave = -1;

        [Range(0.00f, 1.00f)]
        [SerializeField] private float _spawnWeight;

        [Header("Stats")]
        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth;
        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
                _health = Mathf.Clamp(_health, 0, MaxHealth);
                OnHealthValueChanged?.Invoke(_health);
            }
        }

        public float MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
                OnMaxHealthValueChanged?.Invoke(_maxHealth);
            }
        }

        public int MinSpawnWave => _minSpawnWave;
        public int MaxSpawnWave => _maxSpawnWave;
        public float SpawnWeight => _spawnWeight;

        public event Action<float> OnHealthValueChanged;
        public event Action<float> OnMaxHealthValueChanged;
        public event Action OnClear;
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

            const float SCORE_MODIFIER = 100;
            ScoreManager.ModifyScore(MaxHealth * SCORE_MODIFIER);
            OnClear?.Invoke();
            OnDeath?.Invoke();
        }
    }
}
