using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the active enemies
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory _enemyFactory = null;

    public HashSet<EnemyController> Enemies { get; private set; }

    private void Awake()
    {
        Enemies = new(_enemyFactory.NumEnemiesInPool);
    }
    
    public void RegisterEnemy(EnemyController enemy)
    {
        if (!Enemies.Add(enemy))
        {
            Debug.LogWarning($"Already registered enemy: {enemy.name}");
        }
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        if (Enemies.Remove(enemy))
        {
            _enemyFactory.ReturnEnemyToPool(enemy);   
        }
        else
        {
            Debug.LogWarning($"Already unregistered enemy: {enemy.name}");
        }
    }
}
