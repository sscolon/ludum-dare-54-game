using DDCore;
using ProjectBubble.Core.Collectibles;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.Content.Collectibles
{
    public abstract class CollectibleDefault :
        MonoBehaviour,
        ICollectible
    {
        private const float Impulse_Speed = 5;
        private const float Acceleration = 1.5f;
        private const float Deceleration = 1.5f;
        private float _elapsedSpawnTime;
        private float _elapsedCollectibleTime;
        private float _elapsedCollectibleDelay;
        private float _calculatedArcHeight;
        private float _calculatedSpawnTime;
        private Rigidbody2D _rb2D;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _collectibleDelay;
        [SerializeField] private float _collectibleTime;
        [SerializeField] private float _spawnTime;
        [SerializeField] private float _arcHeight;
        [SerializeField] private Material _spriteWhite;
        [SerializeField] private CollectibleState _state;
        protected virtual void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _spriteRenderer.transform.localScale = Vector3.zero;
            CalculateMovement();
        }

        private void FixedUpdate()
        {
            _rb2D?.Move(Vector2.zero, Acceleration, Deceleration);
        }

        private void Update()
        {
            switch (_state)
            {
                case CollectibleState.Spawning:
                    _elapsedSpawnTime += Time.deltaTime;
                    float spawnProgress = _elapsedSpawnTime / _calculatedSpawnTime;
                    float startHeightProgress = Mathf.Clamp01(spawnProgress * 2);
                    float endHeightProgress = Mathf.Clamp01(spawnProgress - 0.5f);

                    float startHeight = Mathf.Lerp(0, _calculatedArcHeight, startHeightProgress);
                    float endHeight = Easing.Calculate(_calculatedArcHeight, 0, endHeightProgress, EaseType.Out_Bounce);
                    float height = Mathf.Lerp(startHeight, endHeight, spawnProgress);

                    _spriteRenderer.transform.localScale = Easing.Calculate(Vector2.zero, Vector2.one, spawnProgress, EaseType.Out_Bounce);
                    _spriteRenderer.transform.localPosition = new(0, height);

                    if (spawnProgress >= 1f)
                    {
                        //TODO: Play Sounds/Effects
                        _elapsedCollectibleDelay += Time.deltaTime;
                        if (_elapsedCollectibleDelay >= _collectibleDelay)
                        {
                            _state = CollectibleState.Collectible;
                        }
                    }
                    break;
                case CollectibleState.Collecting:
                    _elapsedCollectibleTime += Time.deltaTime;
                    Vector2 targetSize = new(4f, 0f);

                    float collectProgress = _elapsedCollectibleTime / _collectibleTime;
                    float easedTime = Easing.Calculate(collectProgress, EaseType.In_Out_Bounce);
                    _spriteRenderer.transform.localScale = Vector2.Lerp(Vector2.one, targetSize, easedTime);

                    if (collectProgress >= 1f)
                    {
                        //TODO: Play Sounds/Effects
                        Destroy(gameObject);
                    }
                    break;
            }
        }

        private void CalculateMovement()
        {
            _calculatedArcHeight = _arcHeight + Random.Range(-1f, 1f);
            _calculatedSpawnTime = _spawnTime + Random.Range(-0.25f, 0.25f);

            float randX = Random.Range(-1f, 1f);
            float randY = Random.Range(-1f, 1f);
            Vector3 applyForce = new Vector3(randX, randY) * Impulse_Speed;
            _rb2D.AddForce(applyForce, ForceMode2D.Impulse);
        }
        public virtual bool CanCollect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            return _state == CollectibleState.Collectible;
        }

        public virtual void Collect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            _state = CollectibleState.Collecting;
            _spriteRenderer.material = _spriteWhite;
        }

        public enum CollectibleState
        {
            Idle,
            Collectible,
            Spawning,
            Collecting
        }
    }
}
