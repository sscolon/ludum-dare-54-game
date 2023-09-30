using DDCore;
using ProjectBubble.Core.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.MainPlayer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Entity
    {
        private Queue<BubbleData> _bubbleQueue;
        private Rigidbody2D _rb2D;
        private float _inputX;
        private float _inputY;
   
        [SerializeField] private BubbleData _starterBubble;
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;

        private void Start()
        {
            _bubbleQueue = new Queue<BubbleData>();
            _bubbleQueue.Enqueue(_starterBubble);
            _rb2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _inputX = Input.GetAxisRaw("Horizontal");
            _inputY = Input.GetAxisRaw("Vertical");
            if (Input.GetMouseButtonDown(0))
            {
                NextBubble();
            }
        }

        private void FixedUpdate()
        {
            Vector2 input = new Vector2(_inputX, _inputY);
            Vector2 normalizedInput = input.normalized;
            Vector2 targetSpeed = normalizedInput * _movementSpeed;
            Move(targetSpeed, _acceleration, _deceleration);
        }

        private void NextBubble()
        {
            if (_bubbleQueue.Count <= 0)
                return;

            BubbleData bubbleData = _bubbleQueue.Dequeue();
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
                }
            }
        }

        private void Move(Vector2 targetSpeed, float acceleration, float deceleration)
        {
            Vector2 velocity = _rb2D.velocity;
            Vector2 diff = targetSpeed - velocity;
            float accelX = (Mathf.Abs(targetSpeed.x) > 0.01f) ? acceleration : deceleration;
            float accelY = (Mathf.Abs(targetSpeed.y) > 0.01f) ? acceleration : deceleration;

            Vector2 movement = Vector2.zero;
            movement.x = Mathf.Abs(diff.x) * accelX * Mathf.Sign(diff.x) * acceleration;
            movement.y = Mathf.Abs(diff.y) * accelY * Mathf.Sign(diff.y) * acceleration;
            _rb2D.AddForce(movement);
        }
    }
}
