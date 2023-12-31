﻿using DDCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectBubble.Core
{
    public class WaveManager : MonoBehaviour
    {
        private static int _lastWaveIndex;
        private int _waveIndex;
        private int _enemyCount;
        private float _enemySpawnCountdown;
        private float _enemySpawnTime;
        private float _elapsedRestTime;
        private State _state;
        private IWaveBehaviour[] _waveBehaviours;
        private Queue<GameObject> _prefabsToInstantiate;

        [SerializeField] private int _maxWave;
        [SerializeField] private int _restWaveDivisor;
        [SerializeField] private float _restTime;

        [Header("Enemy Spawning")]
        [SerializeField] private float _minEnemySpawnTime;
        [SerializeField] private float _maxEnemySpawnTime;
        [SerializeField] private Vector2Int[] _enemyCounts;
        [SerializeField] private GameObject[] _prefabs;
        [SerializeField] private VFX _spawnVFX;

        [Header("Rest Wave")]
        [SerializeField] private int _minBonusShopItemCount;
        [SerializeField] private int _maxBonusShopItemCount;
        [SerializeField] private GameObject[] _requiredShopItemPrefabs;
        [SerializeField] private GameObject[] _bonusShopItemPrefabs;

        public static event Action OnRestStart;
        public static event Action OnRestStop;
        public static event Action<int> OnWaveStart;
        public static event Action<float> OnRestCountdown;
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
            if (Input.GetKeyDown(KeyCode.Y))
            {
                //Just verifiying this works.
                NextWave();
            }

            switch (_state)
            {
                case State.Combat:
                    _enemySpawnCountdown -= Time.deltaTime;
                    if (_enemySpawnCountdown <= 0 && _prefabsToInstantiate.Count > 0)
                    {
                        _enemySpawnCountdown = _enemySpawnTime;
                        SpawnEnemy();
                    }
                    break;
                case State.Rest:
                    _elapsedRestTime += Time.deltaTime;
                    OnRestCountdown?.Invoke(_restTime - _elapsedRestTime);
                    if (_elapsedRestTime >= _restTime)
                    {
                        NextWave();
                    }
                    break;
                default:
                    break;
            }
        }
        public static int GetWave()
        {
            return _lastWaveIndex;
        }

        public void StartWaves()
        {
            NextWave();
        }

        private Vector3Int GetRandomSpawnPosition(float minDistance = 0, float maxDistance = float.MaxValue)
        {
            //TODO: Telegraph it, we'll do this in polishing phase.
            //There'll be a sound effect and particle effect before the enemy spawns.
            Tilemap ground = World.Ground;
            List<Vector3Int> spawnPositions = new();
            Vector3 cameraPos = Util.GetMainCamera().transform.position;
            cameraPos.z = 0.0f;
            Vector3Int cameraTile = Vector3Int.RoundToInt(cameraPos);
            foreach (var cellPosition in ground.cellBounds.allPositionsWithin)
            {
                float distance = Vector3Int.Distance(cellPosition, cameraTile);
                if (ground.HasTile(cellPosition) && distance >= minDistance && distance <= maxDistance)
                {
                    spawnPositions.Add(cellPosition);
                }
            }

            if(spawnPositions.Count == 0)
            {
                foreach (var cellPosition in ground.cellBounds.allPositionsWithin)
                {
                    float distance = Vector3Int.Distance(cellPosition, cameraTile);
                    if (ground.HasTile(cellPosition))
                    {
                        spawnPositions.Add(cellPosition);
                    }
                }
            }

            Vector3Int spawnPosition = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
            return spawnPosition;
        }

        private void SpawnEnemy()
        {
            GameObject prefab = _prefabsToInstantiate.Dequeue();
            GameObject instance = SpawnPrefab(prefab, 2, 10);
            IWaveBehaviour waveBehaviour = instance.GetComponent<IWaveBehaviour>();
            waveBehaviour.OnClear += Progress;
            void Progress()
            {
                waveBehaviour.OnClear -= Progress;
                _enemyCount--;
                if (_enemyCount <= 0)
                {
                    NextWave();
                }
            }
        }

        public GameObject SpawnPrefab(GameObject prefab, float minDistance = 0, float maxDistance = float.MaxValue)
        {
            Vector3Int tilePosition = GetRandomSpawnPosition(minDistance, maxDistance);
            Vector3 worldPosition = tilePosition;
            worldPosition += new Vector3(0.5f, 0.5f);
            GameObject instance = GameObject.Instantiate(prefab, worldPosition, prefab.transform.rotation);
            _spawnVFX?.Play(worldPosition);
            return instance;
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

            float randomWeight = UnityEngine.Random.Range(0, totalWeight);
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
            _lastWaveIndex = _waveIndex;
            _waveIndex++;
            DebugWrapper.Log($"Current Wave {_lastWaveIndex}");
            if (_enemyCounts.Length > _waveIndex)
            {
                int min = _enemyCounts[_waveIndex].x;
                int max = _enemyCounts[_waveIndex].y;
                _enemyCount = UnityEngine.Random.Range(min, max);
            }
            else
            {
                //Default to final wave.
                int min = _enemyCounts[_enemyCounts.Length - 1].x;
                int max = _enemyCounts[_enemyCounts.Length - 1].y;
                _enemyCount = UnityEngine.Random.Range(min, max);
            }

            //Gradually get faster as the waves progress.
            float waveProgress = (_waveIndex + 1) / (float)_maxWave;
            _enemySpawnTime = Mathf.Lerp(_maxEnemySpawnTime, _minEnemySpawnTime, waveProgress);
            if (_waveIndex >= _maxWave)
            {
                //YOU WIN
                GameManager.Victory();
            }
            else
            {
                //We can hardcode shops and stuff here.
                //For now enemies get queued up and will spawn on a timer.
                if (_waveIndex % _restWaveDivisor == 0)
                {
                    _state = State.Rest;
                    _elapsedRestTime = 0;
                    int bonusShopItemCount = UnityEngine.Random.Range(_minBonusShopItemCount, _maxBonusShopItemCount);
                    for (int r = 0; r < _requiredShopItemPrefabs.Length; r++)
                    {
                        SpawnPrefab(_requiredShopItemPrefabs[r], 4);
                    }

                    for (int s = 0; s < bonusShopItemCount; s++)
                    {
                        int rand = UnityEngine.Random.Range(0, _bonusShopItemPrefabs.Length);
                        GameObject prefab = _bonusShopItemPrefabs[rand];
                        SpawnPrefab(prefab, 4);
                    }
                    OnRestStart?.Invoke();
                }
                else
                {
                    if (_state == State.Rest)
                        OnRestStop?.Invoke();
                    OnWaveStart?.Invoke(_waveIndex);
                    _state = State.Combat;
                    int count = _enemyCount;
                    for (int e = 0; e < count; e++)
                    {
                        int index = NextPrefabIndex();
                        if (_prefabs == null || _prefabs.Length <= 0)
                        {
                            _enemyCount--;
                            continue;
                        }

                        GameObject prefab = _prefabs[index];
                        _prefabsToInstantiate.Enqueue(prefab);
                    }

                    if(_enemyCount <= 0)
                    {
                        NextWave();
                    }
                }
            }
        }

        private enum State
        {
            Idle = 0,
            Combat = 1,
            Rest = 2
        }
    }
}
