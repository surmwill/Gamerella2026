using System;
using System.Collections.Generic;
using UnityEngine;

using static EnemyFactory;

[CreateAssetMenu(menuName = MenuNames.GameAssetMenu + "SpawnIntervals Configuration")]
public class SpawnIntervalsConfigurationSO : ScriptableObject
{ 
    [SerializeField]
    private List<SpawnInterval> _spawnIntervals = null;
    
    public List<SpawnInterval> SpawnIntervals => _spawnIntervals;

    private static readonly EnemyType[] EnemyTypes = (EnemyType[]) Enum.GetValues(typeof(EnemyType));
    
    private const float MinIntervalTimeSeconds = 1.0f;

    public void OnValidate()
    {
        float? lastIntervalEndSeconds = null;
        foreach (SpawnInterval spawnInterval in _spawnIntervals)
        {
            // Ensure intervals are sequential and don't overlap
            spawnInterval.StartIntervalSeconds = Mathf.Max(lastIntervalEndSeconds ?? 0, spawnInterval.StartIntervalSeconds);
            spawnInterval.EndIntervalSecondsExclusive = Mathf.Max(spawnInterval.StartIntervalSeconds + MinIntervalTimeSeconds, spawnInterval.EndIntervalSecondsExclusive);
            lastIntervalEndSeconds = spawnInterval.EndIntervalSecondsExclusive;
            
            // Add missing enemy entries
            List<SpawnEnemyType> spawnEnemyTypes = spawnInterval.SpawnEnemyTypes;
            foreach (EnemyType allEnemyTypes in EnemyTypes)
            {
                if (!spawnEnemyTypes.Exists(spawnEnemyType => spawnEnemyType.EnemyType == allEnemyTypes))
                {
                    spawnEnemyTypes.Add(new SpawnEnemyType(allEnemyTypes));
                }
            }

            // Remove duplicates
            HashSet<EnemyType> foundEnemyTypes = new();
            for (int i = 0; i < spawnEnemyTypes.Count; i++)
            {
                SpawnEnemyType spawnEnemyType = spawnEnemyTypes[i];
                spawnEnemyType.NumToSpawn = Mathf.Max(0, spawnEnemyType.NumToSpawn); // Ensure proper spawn values
                
                if (!foundEnemyTypes.Add(spawnEnemyType.EnemyType))
                {
                    spawnEnemyTypes.RemoveAt(i);
                }
            }
            
            // Keep the same ordering of enemies for clarity
            spawnEnemyTypes.Sort(Comparer<SpawnEnemyType>.Create((x, y) => x.EnemyType.CompareTo(y.EnemyType)));
        }
    }
    
    [Serializable]
    public class SpawnInterval
    {
        [SerializeField]
        private float _startIntervalSeconds = 0f;

        [SerializeField]
        private float _endIntervalSecondsExclusive = 30f;

        [SerializeField]
        private List<SpawnEnemyType> _spawnEnemyTypes = null;

        public float StartIntervalSeconds
        {
            get => _startIntervalSeconds;
            set => _startIntervalSeconds = value;
        }

        public float EndIntervalSecondsExclusive
        {
            get =>  _endIntervalSecondsExclusive;
            set => _endIntervalSecondsExclusive = value;
        }

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

        public int NumToSpawn
        {
            get => _numToSpawn;
            set => _numToSpawn = value;
        }
        
        public int NumSpawned { get; set; }

        public int RemainingToSpawn => NumToSpawn - NumSpawned;

        public SpawnEnemyType(EnemyType enemyType, int numToSpawn = 0)
        {
            _enemyType = enemyType;
            _numToSpawn = numToSpawn;
        }
    }
}