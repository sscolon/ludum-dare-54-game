using System;
using UnityEngine;

namespace ProjectBubble.Core
{
    public static class PauseManager
    {
        public static event Action OnPause;
        public static event Action OnUnPause;
        public static void Pause()
        {
            Time.timeScale = 0;
            OnPause?.Invoke();
        }

        public static void UnPause()
        {
            Time.timeScale = 1;
            OnUnPause?.Invoke();
        }
    }
}
