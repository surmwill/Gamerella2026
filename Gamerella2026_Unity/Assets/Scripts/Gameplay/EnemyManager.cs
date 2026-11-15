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

    public Dictionary<Collider2D, EnemyController> EnemiesByCollider { get; private set; }

    private void Awake()
    {
        Enemies = new(_enemyFactory.NumEnemiesInPool);
        EnemiesByCollider = new(_enemyFactory.NumEnemiesInPool);
    }
    
    public void RegisterEnemy(EnemyController enemy)
    {
        if (Enemies.Add(enemy))
        {
            EnemiesByCollider[enemy.Collider] = enemy;
        }
        else
        {
            Debug.LogWarning($"Already registered enemy: {enemy.name}");
        }
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        if (Enemies.Remove(enemy))
        {
            EnemiesByCollider.Remove(enemy.Collider);
            _enemyFactory.ReturnEnemyToPool(enemy);
        }
        else
        {
            Debug.LogWarning($"Already unregistered enemy: {enemy.name}");
        }
    }
}
