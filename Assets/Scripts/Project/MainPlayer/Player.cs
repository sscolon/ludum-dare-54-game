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
        private Queue<BubbleData> _bubbleQueue;
        private Rigidbody2D _rb2D;
        private Coroutine _tauntRoutine;
        private float _inputX;
        private float _inputY;
 
        private Dictionary<int, int> _collectibleIndex;
        [SerializeField] private BubbleData _starterBubble;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;

        [Header("Scoring")]
        [SerializeField] private float _damagePenalty;

        [Header("Collecting")]
        [SerializeField] private float _collectRadius = 1;

        [Header("VFX")]
        [SerializeField] private Transform _tauntTransform;
        [SerializeField] private Transform _flipperTransform;

        public event Action<BubbleData> OnQueue;
        public event Action<BubbleData> OnDequeue;
        public event Action<Dictionary<int, int>> OnCollect;
        private void Start()
        {
            _collectibleIndex = new();
            _bubbleQueue = new();
            _bubbleQueue.Enqueue(_starterBubble);
            _rb2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _inputX = Input.GetAxisRaw("Horizontal");
            _inputY = Input.GetAxisRaw("Vertical");
            Vector3 mouseWorld = Util.GetMouseWorldPosition();
            if(mouseWorld.x < transform.position.x)
            {
                _flipperTransform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                _flipperTransform.localScale = Vector3.one;
            }

            if (Input.GetMouseButtonDown(0))
            {
                NextBubble();
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_tauntRoutine != null)
                    StopCoroutine(_tauntRoutine);
                _tauntRoutine = StartCoroutine(Taunt());
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

        private IEnumerator Taunt()
        {
            const float Taunt_Speed = 3f;
            float elapsedTime = 0f;
            while(elapsedTime < 1f)
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
            for(int i = 0; i < collisions.Length; i++)
            {
                Collider2D collision = collisions[i];
                if (collision.gameObject == gameObject)
                    continue;
                if(collision.gameObject.TryGetComponent(out ICollectible collectible) && collectible.CanCollect(gameObject, _collectibleIndex))
                {
                    collectible.Collect(gameObject, _collectibleIndex);
                    OnCollect?.Invoke(_collectibleIndex);
                }
            }
        }

        public void NewBubble(BubbleData bubbleData)
        {
            //We have to clone it so that we can have duplicates of the same bubble type.
            BubbleData instance = Instantiate(bubbleData);
            instance.name = bubbleData.name;
            _bubbleQueue.Enqueue(instance);
            OnQueue?.Invoke(instance);
            //VFX HERE
        }

        private void NextBubble()
        {
            if (_bubbleQueue.Count <= 0)
                return;

            BubbleData bubbleData = _bubbleQueue.Dequeue();
            OnDequeue?.Invoke(bubbleData);

            GameObject instance = Instantiate(bubbleData.prefab, transform.position, bubbleData.prefab.transform.rotation);
            Bubble bubble = instance.GetComponent<Bubble>();
            if(bubble == null)
            {
                DebugWrapper.LogError("You forgot to add the bubble reference, IDIOT!");
                return;
            }

            Vector3 mouseWorld = Util.GetMouseWorldPosition();
            if (bubbleData.HasData())
            {
                bubble.Fire(bubbleData, Bubble.Type.Release, mouseWorld);
                bubble.OnRelease += Release;
                void Release(BubbleData bubbleData)
                {
                    bubble.OnRelease -= Release;
                    _bubbleQueue.Enqueue(bubbleData);
                    OnQueue?.Invoke(bubbleData);
                }
            }
            else
            {
                bubble.Fire(bubbleData, Bubble.Type.Catch, mouseWorld);
                bubble.OnCatch += Catch;
                void Catch(BubbleData bubbleData)
                {
                    bubble.OnCatch -= Catch;
                    _bubbleQueue.Enqueue(bubbleData);
                    OnQueue?.Invoke(bubbleData);
                }
            }
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
