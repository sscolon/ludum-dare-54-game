using DDCore;
using System;
using UnityEngine;

namespace ProjectBubble.Core
{
    public static class GameManager
    {
        public const string Main_Scene = "Main";

        public static event Action OnVictory;
        public static event Action OnLose;
        public static void Victory()
        {
            DebugWrapper.Log("Victory!");
            OnVictory?.Invoke();
        }

        public static void Lose()
        {
            DebugWrapper.Log("Dead XD!");
            OnLose?.Invoke();
        }

        public static void Restart()
        {
            SceneLoader.Main.LoadScene(Main_Scene);
        }
    }
}
