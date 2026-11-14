using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    private Transform _enemiesPool = null;

    [SerializeField]
    private List<Transform> _enemySpawnPoints = null;

    [SerializeField]
    private EnemyController _enemyPrefab = null;

    [SerializeField]
    private int _numEnemiesInPool = 10;
    
    public int NumEnemiesInPool => _numEnemiesInPool;

    public EnemyController SpawnEnemy(EnemyType enemyType)
    {
        EnemyController enemy = null;
        if (_enemiesPool.childCount > 0)
        {
            enemy = _enemiesPool.GetChild(0).GetComponent<EnemyController>();
        }
        else
        {
            Debug.LogWarning("Enemy pool empty, using fresh instantiation");
            enemy = Instantiate(_enemyPrefab, _enemiesPool);
            enemy.gameObject.SetActive(false);
        }

        Transform spawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
        enemy.transform.SetParent(spawnPoint);
        enemy.transform.localPosition = Vector3.zero;

        return enemy;
    }

    public void ReturnEnemyToPool(EnemyController enemy)
    {   
        enemy.transform.SetParent(_enemiesPool);
        enemy.gameObject.SetActive(false);
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        _numEnemiesInPool = Mathf.Max(_numEnemiesInPool, 0);
        
        if (_enemiesPool == null || _enemyPrefab == null)
        {
            return;
        }
        
        for (int i = _enemiesPool.childCount; i < _numEnemiesInPool; i++)
        {
            EnemyController enemy = (EnemyController) PrefabUtility.InstantiatePrefab(_enemyPrefab);
            enemy.transform.SetParent(_enemiesPool);
            enemy.gameObject.SetActive(false);
        }

        EditorApplication.delayCall += () =>
        {
            for (int i = _enemiesPool.childCount; i > _numEnemiesInPool; i--)
            {
                DestroyImmediate(_enemiesPool.GetChild(0).gameObject);
            }
        };
    }
    #endif
    
    public enum EnemyType
    {
        // Comment = value of each chip
        White = 0, // 1
        Red = 1, // 5
        Green = 2, // 25
        Blue = 3, // 50
        Black = 4 // 100
    }
}
