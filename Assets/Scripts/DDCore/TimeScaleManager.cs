using System;
using System.Collections.Generic;
using UnityEngine;

namespace DDCore
{
    public static class TimeScaleManager
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Reset();
        }

        private static int _pauseCount;
        private static Dictionary<string, float> _timeScaleIndexBacking;
        private static Dictionary<string, float> _timeScaleIndex
        {
            get
            {
                if (_timeScaleIndexBacking == null)
                    _timeScaleIndexBacking = new Dictionary<string, float>();
                return _timeScaleIndexBacking;
            }
        }

        public static float BaseTimeScale { get; set; } = 1f;
        public static float WorldTimeScale
        {
            get
            {
                float timeScale = BaseTimeScale;
                foreach (var key in _timeScaleIndex)
                {
                    timeScale *= key.Value;
                }
                return timeScale;
            }
        }

        public static float TotalTimeScale
        {
            get
            {
                float pauseMultiplier = IsPaused() ? 0f : 1f;
                return Mathf.Clamp(WorldTimeScale * pauseMultiplier, 0f, float.MaxValue);
            }
        }

        public static event Action OnApply;
        public static float GetScale(string key)
        {
            if (!_timeScaleIndex.ContainsKey(key))
                return 1f;
            else
                return _timeScaleIndex[key];
        }

        public static void SetScale(string key, float multiplier)
        {
            if (!_timeScaleIndex.ContainsKey(key))
            {
                _timeScaleIndex.Add(key, multiplier);
            }
            else
            {
                _timeScaleIndex[key] = multiplier;
            }
        }

        public static void Reset()
        {
            _pauseCount = 0;
            _timeScaleIndex.Clear();
            Apply();
        }

        public static void AddPause()
        {
            _pauseCount++;
            Apply();
        }

        public static void RemovePause()
        {
            _pauseCount--;
            if (_pauseCount < 0)
                _pauseCount = 0;
            Apply();
        }

        public static bool IsPaused()
        {
            return _pauseCount > 0;
        }

        public static void Apply()
        {
            Time.timeScale = TotalTimeScale;
            bool isPaused = IsPaused();
            /*if (isPaused)
            {
                Controls.Pause();
            }
            else
            {
                Controls.UnPause();
            }*/

            OnApply?.Invoke();
        }
    }
}
