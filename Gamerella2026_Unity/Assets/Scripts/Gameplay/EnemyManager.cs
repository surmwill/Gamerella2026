using System;
using System.Collections.Generic;
using UnityEngine;
using static GameModifiers;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory _enemyFactory = null;

    public HashSet<EnemyController> Enemies { get; private set; }

    private void Awake()
    {
        Enemies = new(_enemyFactory.NumEnemiesInPool);
    }

    public void SpawnEnemy()
    {

    }

    private void RegisterEnemy(EnemyController enemy)
    {
        if (Enemies.Add(enemy))
        {
        }
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        if (Enemies.Remove(enemy))
        {
            _enemyFactory.ReturnEnemyToPool(enemy);   
        }
    }

    public void ApplyEnemyModifier(ModifierType modifierType)
    {
        
    }

    private void Update()
    {
        
    }
}
