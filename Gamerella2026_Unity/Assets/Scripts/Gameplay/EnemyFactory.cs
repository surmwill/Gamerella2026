using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    private Transform _enemiesPool = null;

    [SerializeField]
    private List<Transform> _enemySpawnPoints = null;

    [SerializeField]
    private GameObject _enemyPrefab = null;

    public void SpawnEnemy(EnemyType enemyType)
    {
        
    }

    public enum EnemyType
    {
        White = 0,
        Red = 1,
        Green = 2,
        Blue = 3,
        Black = 4
    }
}
