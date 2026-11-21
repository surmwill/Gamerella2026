using System;
using System.Collections.Generic;
using UnityEngine;

using static EnemyFactory;

[CreateAssetMenu(menuName = MenuNames.GameAssetMenu + "SpawnIntervalsSO")]
public class SpawnIntervalsConfigurationSO : MonoBehaviour
{ 
    [SerializeField]
    private List<SpawnInterval> _spawnIntervals = null;
    
    public List<SpawnInterval> SpawnIntervals => _spawnIntervals;
    
    [Serializable]
    public class SpawnInterval
    {
        [SerializeField]
        private float _startIntervalSeconds = 0f;

        [SerializeField]
        private float _endIntervalSecondsExclusive = 30f;

        [SerializeField]
        private List<SpawnEnemyType> _spawnEnemyTypes = null;
        
        public float StartIntervalSeconds => _startIntervalSeconds;
        
        public float EndIntervalSecondsExclusive => _endIntervalSecondsExclusive;

        public List<SpawnEnemyType> SpawnEnemyTypes => _spawnEnemyTypes;

        public int TotalToSpawn
        {
            get
            {
                // Avoid LINQ because this is expected to be called in Update
                int totalToSpawn = 0;
                foreach (SpawnEnemyType spawnEnemyType in SpawnEnemyTypes)
                {
                    totalToSpawn += spawnEnemyType.NumToSpawn;
                }
                return totalToSpawn;
            }
        }
        
        public int TotalSpawned
        {
            get
            {
                // Avoid LINQ because this is expected to be called in Update
                int totalSpawned = 0;
                foreach (SpawnEnemyType spawnEnemyType in SpawnEnemyTypes)
                {
                    totalSpawned += spawnEnemyType.NumSpawned;
                }
                return totalSpawned;
            }
        }

        public float PctCompletion => Mathf.InverseLerp(StartIntervalSeconds, EndIntervalSecondsExclusive, Game.Instance.ElapsedTimeSeconds);
        
        public SpawnInterval(float startIntervalSeconds, float endIntervalSecondsExclusive, List<SpawnEnemyType> spawnEnemyTypes)
        {
            _startIntervalSeconds = startIntervalSeconds;
            _endIntervalSecondsExclusive = endIntervalSecondsExclusive;
            _spawnEnemyTypes = spawnEnemyTypes;
        }
    }

    [Serializable]
    public class SpawnEnemyType
    {
        [SerializeField]
        private EnemyType _enemyType = default;

        [SerializeField]
        private int _numToSpawn;

        public EnemyType EnemyType => _enemyType;

        public int NumToSpawn => _numToSpawn;
        
        public int NumSpawned { get; set; }

        public int RemainingToSpawn => NumToSpawn - NumSpawned;

        public SpawnEnemyType(EnemyType enemyType, int numToSpawn)
        {
            _enemyType = enemyType;
            _numToSpawn = numToSpawn;
        }
    }
}
