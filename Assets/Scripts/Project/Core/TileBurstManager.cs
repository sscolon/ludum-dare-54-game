using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBubble.Core
{
    public class TileBurstManager : MonoBehaviour
    {
        private static TileBurstManager _instance;
        private HashSet<Vector3Int> _usedTiles;
        [SerializeField] private TileBurstAnimation _tileBurstAnimationPrefab;
        private void OnEnable()
        {
            _instance = this;
            _usedTiles = new();
        }
        public static void UseTile(Vector3Int tilePosition)
        {
            TileBurstManager tileBurstManager = _instance;
            tileBurstManager._usedTiles.Add(tilePosition);
        }

        public static void FreeTile(Vector3Int tilePosition)
        {
            TileBurstManager tileBurstManager = _instance;
            tileBurstManager._usedTiles.Remove(tilePosition);
        }

        public static bool IsUsed(Vector3Int tilePosition)
        {
            TileBurstManager tileBurstManager = _instance;
            return tileBurstManager._usedTiles.Contains(tilePosition);
        }

        public static void Burst(Vector3 startPosition, Vector3 targetPosition, Action onComplete)
        {
            TileBurstManager tileBurstManager = _instance;
            TileBurstAnimation prefab = tileBurstManager._tileBurstAnimationPrefab;
            TileBurstAnimation instance = Instantiate(prefab, startPosition, prefab.transform.rotation);
            instance.StartPosition = startPosition;
            instance.TargetPosition = targetPosition;
            instance.OnComplete = onComplete;
        }
    }
}
