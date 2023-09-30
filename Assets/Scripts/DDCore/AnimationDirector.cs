using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDCore
{ 
    public class AnimationDirector : MonoBehaviour
    {
        public const float FRAME_RATE_MILLISECONDS = 50;

        [Serializable]
        public class AnimationDirectorEvent
        {
            public int frame;
            public bool hasTrigged;
            public Action @event;
        }

        [Serializable]
        public class Animation
        {
            public string name;
            public bool isLooping;
            public Sprite[] sprites;
            public List<AnimationDirectorEvent> events = new List<AnimationDirectorEvent>();
            public int FrameCount => sprites.Length;
            public Animation(string name, Sprite[] sprites)
            {
                this.name = name;
                this.sprites = sprites;
                this.isLooping = false;
                this.events = new List<AnimationDirectorEvent>();
            }
        }

        private int _frame;

        [Header("Editor Purposes Only")]
        [SerializeField]
        private Animation _animation;
        private SpriteRenderer _spriteRendererBacking;
        private SpriteRenderer _spriteRenderer
        {
            get
            {
                if (_spriteRendererBacking == null)
                    _spriteRendererBacking = GetComponentInChildren<SpriteRenderer>();
                return _spriteRendererBacking;
            }
        }

        [Header("Animations")]
        [SerializeField] private string _defaultAnimation;
        [SerializeField] private List<Animation> _animations;
        public float animationSpeed = 1f;

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(_defaultAnimation))
                PlayAbsolute(_defaultAnimation);
            ResetState();
            StartCoroutine(UpdateFrame());
        }

        private void ResetState()
        {
            _frame = 0;
            UpdateSprite();
            ResetEvents();
        }

        private IEnumerator UpdateFrame()
        {
            while (true)
            {
                //Just incase animators aren't animating.
                if (_animation == null)
                {
                    yield return null;
                    continue;
                }


                //So
                //I wanna try some sort of tweening.

                UpdateSprite();
                for (int i = 0; i < _animation.events.Count; i++)
                {
                    AnimationDirectorEvent animationEvent = _animation.events[i];
                    if (animationEvent.frame <= _frame && !animationEvent.hasTrigged)
                    {
                        animationEvent.hasTrigged = true;
                        animationEvent.@event();
                    }
                }


                //Coroutines should keep the frame rate consistent
                //So we don't need to do any math to correct the current frame when people have vsync off.
                float targetWaitTime = (FRAME_RATE_MILLISECONDS / 1000);
                float time = 0f;
                while (time < targetWaitTime)
                {
                    time += Time.deltaTime * animationSpeed;
                    yield return null;
                }

                _frame += 1;
                if (_frame >= _animation.FrameCount)
                {
                    if (_animation.isLooping)
                    {
                        _frame = 0;
                        for (int i = 0; i < _animation.events.Count; i++)
                        {
                            AnimationDirectorEvent animationEvent = _animation.events[i];
                            animationEvent.hasTrigged = false;
                        }
                    }
                }
            }
        }

        private void ResetEvents()
        {
            for (int a = 0; a < _animations.Count; a++)
            {
                for (int e = 0; e < _animations[a].events.Count; e++)
                {
                    _animations[a].events[e].hasTrigged = false;
                }
            }
        }


        public void Play(string animationName, int frame = 0)
        {
            if (_animation != null && _animation.name == animationName)
                return;

            Animation animation = GetAnimation(animationName);
            if (animation == null)
                return;

            _animation = animation;
            _frame = frame;

            UpdateSprite();
            ResetEvents();
        }

        public void PlayLastFrame(string animationName)
        {
            Play(animationName, GetLastFrame(animationName));
        }

        public void PlayAbsolute(string animationName)
        {
            Animation animation = GetAnimation(animationName);
            if (animation == null)
                return;

            _animation = animation;
            _frame = 0;
            _spriteRenderer.sprite = _animation.sprites[_frame];

            UpdateSprite();
            ResetEvents();
        }

        private void UpdateSprite()
        {
            if (_animation == null)
                return;
            if (_animation.sprites.Length <= _frame)
                return;
            _spriteRenderer.sprite = _animation.sprites[_frame];
        }

        public void AddEvent(string animationName, int frame, Action @event)
        {
            AnimationDirectorEvent animationEvent = new AnimationDirectorEvent
            {
                frame = frame,
                hasTrigged = false,
                @event = @event
            };

            Animation animation = GetAnimation(animationName);
            if (animation == null)
                return;

            animation.events.Add(animationEvent);
        }

        public void AddEventAtEnd(string animationName, Action @event)
        {
            AnimationDirectorEvent animationEvent = new AnimationDirectorEvent
            {
                frame = GetLastFrame(animationName),
                hasTrigged = false,
                @event = @event
            };

            Animation animation = GetAnimation(animationName);
            if (animation == null)
                return;

            animation.events.Add(animationEvent);
        }

        public int GetLastFrame(string animationName)
        {
            Animation animation = GetAnimation(animationName);
            if (animation == null)
                return -1;

            return animation.FrameCount - 1;
        }


        public void AddAnimation(Animation animation)
        {
            if (_animations == null)
                _animations = new List<Animation>();
            _animations.Add(animation);
        }

        private Animation GetAnimation(string name)
        {
            for (int i = 0; i < _animations.Count; i++)
            {
                Animation animation = _animations[i];
                if (animation.name == name)
                    return animation;
            }

            //   DebugWrapper.LogWarning($"No animation with name {name}");
            return null;
        }
    }
}