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
        private float _elapsedSpawnTime;
        private float _elapsedCollectibleTime;
        private float _elapsedCollectibleDelay;
        private Rigidbody2D _rb2D;
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _collectibleDelay;
        [SerializeField] private float _collectibleTime;
        [SerializeField] private float _spawnTime;
        [SerializeField] private float _arcHeight;
        [SerializeField] private CollectibleState _state;
        protected virtual void Start()
        {
            _rb2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _rb2D?.Move(Vector2.zero, 4, 4);
        }

        private void Update()
        {
            switch (_state)
            {
                case CollectibleState.Spawning:
                    _elapsedSpawnTime += Time.deltaTime;
                    float spawnProgress = _elapsedSpawnTime / _spawnTime;
                    float startHeightProgress = Mathf.Clamp01(spawnProgress * 2);
                    float endHeightProgress = Mathf.Clamp01(spawnProgress - 0.5f);

                    float startHeight = Mathf.Lerp(0, _arcHeight, startHeightProgress);
                    float endHeight = Easing.Calculate(_arcHeight, 0, endHeightProgress, EaseType.Out_Bounce);
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

        public virtual bool CanCollect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            return _state == CollectibleState.Collectible;
        }

        public virtual void Collect(GameObject gameObject, Dictionary<int, int> collectibleIndex)
        {
            _state = CollectibleState.Collecting;
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
