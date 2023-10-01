using DDCore;
using System;
using UnityEngine;

namespace ProjectBubble.Core
{
    public static class GameManager
    {
        public const string Main_Scene = "Main";

        public static GameObject Player { get; set; }
        public static event Action OnVictory;
        public static event Action OnLose;

        public static Vector3 DirectionToPlayer(Vector3 position)
        {
            Vector3 direction = Player.transform.position - position;
            direction = direction.normalized;
            return direction;
        }
        public static Quaternion RotationToPlayer(Vector3 position)
        {
            return Util.GetAngle(DirectionToPlayer(position));
        }

        public static float DistanceToPlayer(Vector3 position)
        {
            return Vector3.Distance(position, Player.transform.position);
        }

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
