using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyManager _enemyManager = null;

    [SerializeField]
    private List<SpawnInterval> _spawnIntervals = null;

    private SpawnInterval CurrSpawnInterval => _spawnIntervals[_currSpawnIntervalIndex];

    private int _currSpawnIntervalIndex = 0;

    private float? _timeOfLastSpawn;

    private void Update()
    {
        if (Game.Instance.ElapsedTimeSeconds >= CurrSpawnInterval.EndIntervalSecondsExclusive)
        {
            _currSpawnIntervalIndex = Mathf.Min(_currSpawnIntervalIndex + 1, _spawnIntervals.Count - 1);
        }

        if (!_timeOfLastSpawn.HasValue)
        {
            
        }
        
        if (_timeOfLastSpawn > )
    }
    

    [Serializable]
    public class SpawnInterval
    {
        [SerializeField]
        private float _startIntervalSeconds = 0f;

        [SerializeField]
        private float _endIntervalSecondsExclusive = 1f;

        [SerializeField]
        private float EnemySpawnRateSeconds = 0.1f;
        
        public float StartIntervalSeconds => _startIntervalSeconds;
        
        public float EndIntervalSecondsExclusive => _endIntervalSecondsExclusive;
    }
}
