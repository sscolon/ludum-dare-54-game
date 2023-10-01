using System;
using UnityEngine;

namespace ProjectBubble.Core
{
    public static class PauseManager
    {
        private static bool _isPaused;
        public static event Action OnPause;
        public static event Action OnUnPause;
        public static void Pause()
        {
            _isPaused = true;
            Time.timeScale = 0;
            OnPause?.Invoke();
        }

        public static void UnPause()
        {
            _isPaused = false;
            Time.timeScale = 1;
            OnUnPause?.Invoke();
        }

        public static bool IsPaused()
        {
            return _isPaused;
        }
    }
}
