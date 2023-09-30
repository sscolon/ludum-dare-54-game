using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectBubble.Core
{
    public class WaveManager : MonoBehaviour
    {
        private int _waveIndex;
        private int _enemyCount;
        private float _enemySpawnCountdown;
        private float _enemySpawnTime;
        private bool _hasStarted;
        private IWaveBehaviour[] _waveBehaviours;
        private Queue<GameObject> _prefabsToInstantiate;
        [SerializeField] private int _maxWave;
        [SerializeField] private Vector2Int[] _enemyCounts;
        [SerializeField] private float _minEnemySpawnTime;
        [SerializeField] private float _maxEnemySpawnTime;
        [SerializeField] private GameObject[] _prefabs;
        private void Start()
        {
            _prefabsToInstantiate = new Queue<GameObject>();
            _waveBehaviours = new IWaveBehaviour[_prefabs.Length];
            for (int i = 0; i < _prefabs.Length; i++)
            {
                if (_prefabs[i].TryGetComponent(out IWaveBehaviour waveBehaviour))
                {
                    _waveBehaviours[i] = waveBehaviour;
                }
            }
        }

        private void Update()
        {
            if (!_hasStarted)
                return;
            _enemySpawnCountdown -= Time.deltaTime;
            if (_enemySpawnCountdown <= 0 && _prefabsToInstantiate.Count > 0)
            {
                _enemySpawnCountdown = _enemySpawnTime;
                SpawnEnemy();
            }
        }

        public void StartWaves()
        {
            _hasStarted = true;
            NextWave();
        }

        private void SpawnEnemy()
        {
            //TODO: Telegraph it, we'll do this in polishing phase.
            //There'll be a sound effect and particle effect before the enemy spawns.
            Tilemap ground = Ground.Map;
            List<Vector3Int> spawnPositions = new();
            foreach (var cellPosition in ground.cellBounds.allPositionsWithin)
            {
                if (ground.HasTile(cellPosition))
                {
                    spawnPositions.Add(cellPosition);
                }
            }

            Vector3Int spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];
            GameObject prefab = _prefabsToInstantiate.Dequeue();
            GameObject instance = Instantiate(prefab, spawnPosition, prefab.transform.rotation);
            IWaveBehaviour waveBehaviour = instance.GetComponent<IWaveBehaviour>();
            waveBehaviour.OnClear += Progress;
            void Progress()
            {
                waveBehaviour.OnClear -= Progress;
                _enemyCount--;
                if(_enemyCount <= 0)
                {
                    NextWave();
                }
            }
        }

        private bool CanSpawn(IWaveBehaviour waveBehaviour)
        {
            if (_waveIndex < waveBehaviour.MinSpawnWave && waveBehaviour.MinSpawnWave != -1)
                return false;
            if (_waveIndex > waveBehaviour.MaxSpawnWave && waveBehaviour.MaxSpawnWave != -1)
                return false;
            return true;
        }
        private int NextPrefabIndex()
        {
            float totalWeight = 0;
            for (int i = 0; i < _waveBehaviours.Length; i++)
            {
                IWaveBehaviour waveBehaviour = _waveBehaviours[i];
                if (CanSpawn(waveBehaviour))
                {
                    totalWeight += waveBehaviour.SpawnWeight * 100;
                }               
            }

            float randomWeight = Random.Range(0, totalWeight);
            float currentWeight = 0;
            for (int i = 0; i < _waveBehaviours.Length; i++)
            {
                IWaveBehaviour waveBehaviour = _waveBehaviours[i];
                if (CanSpawn(waveBehaviour))
                {
                    currentWeight += waveBehaviour.SpawnWeight * 100;
                    if (randomWeight <= currentWeight)
                    {
                        return i;
                    }
                }
            }

            return 0;
        }

        public void NextWave()
        {
            _waveIndex++;
            if(_enemyCounts.Length > _waveIndex)
            {
                int min = _enemyCounts[_waveIndex].x;
                int max = _enemyCounts[_waveIndex].y;
                _enemyCount = Random.Range(min, max);
            }
            else
            {
                //Default to final wave.
                int min = _enemyCounts[_enemyCounts.Length - 1].x;
                int max = _enemyCounts[_enemyCounts.Length - 1].y;
                _enemyCount = Random.Range(min, max);
            }
  
            //Gradually get faster as the waves progress.
            float waveProgress = (_waveIndex + 1) / (float)_maxWave;
            _enemySpawnTime = Mathf.Lerp(_maxEnemySpawnTime, _minEnemySpawnTime, waveProgress);
            if (_waveIndex >= _maxWave)
            {
                //YOU WIN
            }
            else
            {
                //We can hardcode shops and stuff here.
                //For now enemies get queued up and will spawn on a timer.
                for (int e = 0; e < _enemyCount; e++)
                {
                    int index = NextPrefabIndex();
                    GameObject prefab = _prefabs[index];
                    _prefabsToInstantiate.Enqueue(prefab);
                }
            }
        }
    }
}
