using DDCore;
using ProjectBubble.Core;
using ProjectBubble.Core.Collectibles;
using ProjectBubble.Core.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.MainPlayer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Entity,
        ICollector
    {
        private Dictionary<int, int> _collectibleIndex;
        private Queue<Bubble> _bubbleQueue;
        private List<Bubble> _orbitingBubbles;
        private Rigidbody2D _rb2D;
        private Coroutine _tauntRoutine;
        private float _inputX;
        private float _inputY;
        private float _elapsedIdleTime;
        private float _deltaOffset;

        [SerializeField] private BubbleData _starterBubble;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;

        [Header("Scoring")]
        [SerializeField] private float _damagePenalty;

        [Header("Collecting")]
        [SerializeField] private float _collectRadius = 1;

        [Header("VFX")]
        [SerializeField] private Transform _idleTransform;
        [SerializeField] private Transform _tauntTransform;
        [SerializeField] private Transform _flipperTransform;

        public event Action<Bubble> OnQueue;
        public event Action<Bubble> OnDequeue;
        public event Action<Dictionary<int, int>> OnCollect;
        private void Start()
        {
            _orbitingBubbles = new();
            _collectibleIndex = new();
            _bubbleQueue = new();
            _rb2D = GetComponent<Rigidbody2D>();
            NewBubble(_starterBubble);
        }

        private void Update()
        {
            SetInput();
            SetIdleScale();
            SetFlipperScale();
            SetOrbitingBubblePositions();
            if (Input.GetMouseButtonDown(0))
            {
                NextBubble();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_tauntRoutine != null)
                    StopCoroutine(_tauntRoutine);
                _tauntRoutine = StartCoroutine(Taunt());
                ProjectBubble.Core.MainCamera.Screenshake(1, 0.5f);
            }
        }

        private void FixedUpdate()
        {
            Vector2 input = new Vector2(_inputX, _inputY);
            Vector2 normalizedInput = input.normalized;
            Vector2 targetSpeed = normalizedInput * _movementSpeed;
            _rb2D.Move(targetSpeed, _acceleration, _deceleration);

            //Just call it every frame, overlap circle isn't expensive anyway.
            Collect();
        }

        private void SetOrbitingBubblePositions()
        {
            const float Orbiting_Speed = 200;
            const float Orbiting_Radius = 1.5f;
            _deltaOffset += Time.deltaTime * Orbiting_Speed;
            for (int i = 0; i < _orbitingBubbles.Count; i++)
            {
                Bubble bubble = _orbitingBubbles[i];
                float angle = i * (360f / _orbitingBubbles.Count);
                Vector3 direction = Quaternion.Euler(0, 0, angle + _deltaOffset) * Vector3.up;
                Vector3 position = transform.position + direction * Orbiting_Radius;
                bubble.TargetPosition = position;
            }
        }

        private void SetInput()
        {
            _inputX = Input.GetAxisRaw("Horizontal");
            _inputY = Input.GetAxisRaw("Vertical");
        }

        private void SetIdleScale()
        {
            _elapsedIdleTime += Time.deltaTime;
            float pingPong = Mathf.PingPong(_elapsedIdleTime, 1);
            float easedPingPong = Easing.Calculate(pingPong, EaseType.In_Out_Cubic);
            _idleTransform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.1f, 0.9f, 1f), easedPingPong);
        }

        private void SetFlipperScale()
        {
            Vector3 mouseWorld = Util.GetMouseWorldPosition();
            if (mouseWorld.x < transform.position.x)
            {
                _flipperTransform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                _flipperTransform.localScale = Vector3.one;
            }
        }
        private IEnumerator Taunt()
        {
            const float Taunt_Speed = 3f;
            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * Taunt_Speed;
                float easedTime = Easing.Calculate(elapsedTime, EaseType.Out_Cubic);
                Vector3 startScale = new Vector3(3f, 0f, 0f);
                Vector3 endScale = Vector3.one;

                Vector3 scale = Vector3.Lerp(startScale, endScale, easedTime);
                _tauntTransform.localScale = scale;
                yield return null;
            }
        }

        private void Collect()
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, _collectRadius);
            for (int i = 0; i < collisions.Length; i++)
            {
                Collider2D collision = collisions[i];
                if (collision.gameObject == gameObject)
                    continue;
                if (collision.gameObject.TryGetComponent(out ICollectible collectible) && collectible.CanCollect(gameObject, _collectibleIndex))
                {
                    collectible.Collect(gameObject, _collectibleIndex);
                    OnCollect?.Invoke(_collectibleIndex);
                }
            }
        }

        public void NewBubble(BubbleData bubbleData)
        {
            //We have to clone it so that we can have duplicates of the same bubble type.
            GameObject instance = Instantiate(bubbleData.prefab, transform.position, bubbleData.prefab.transform.rotation);
            Bubble bubble = instance.GetComponent<Bubble>();
            bubble.OnCatch += OnCatchBubble;
            bubble.OnRelease += OnReleaseBubble;
            bubble.MoveType = Bubble.Movement.Return;
            _bubbleQueue.Enqueue(bubble);
            _orbitingBubbles.Add(bubble);
            OnQueue?.Invoke(bubble);
            //VFX HERE
        }

        private void OnReleaseBubble(Bubble bubble)
        {
            _bubbleQueue.Enqueue(bubble);
            _orbitingBubbles.Add(bubble);
        }

        private void OnCatchBubble(Bubble bubble)
        {
            _bubbleQueue.Enqueue(bubble);
            _orbitingBubbles.Add(bubble);
        }

        private void NextBubble()
        {
            if (_bubbleQueue.Count <= 0)
                return;

            Bubble bubble = _bubbleQueue.Dequeue();
            _orbitingBubbles.Remove(bubble);
            OnDequeue?.Invoke(bubble);

            Vector3 mouseWorld = Util.GetMouseWorldPosition();
            Bubble.Type bubbleType = bubble.HasCatch() ? Bubble.Type.Release : Bubble.Type.Catch;
            bubble.Fire(bubbleType, mouseWorld);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            ScoreManager.ModifyScore(_damagePenalty);
        }

        public override void Death()
        {
            base.Death();
            GameManager.Lose();
        }
    }
}
