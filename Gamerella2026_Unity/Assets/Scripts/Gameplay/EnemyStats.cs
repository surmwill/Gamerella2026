using System;
using UnityEngine;

using static EnemyConfigurationsSO;

[Serializable]
public class EnemyStats
{
    [SerializeField]
    private int _health = 0;

    [SerializeField]
    private int _attackDamage = 0;

    [SerializeField]
    private float _moveVelocity = 0f;
    
    [SerializeField]
    private int _tokensDropped = 0;

    public void Init(EnemyBaseStats enemyBaseStats)
    {
        Health = enemyBaseStats.BaseHealth;
        AttackDamage = enemyBaseStats.BaseAttackDamage;
        MoveVelocity = enemyBaseStats.BaseMoveVelocity;
        TokensDropped = enemyBaseStats.TokensDropped;
    }

    public int Health
    {
        get => _health;
        set => _health = value;
    }
    
    public int AttackDamage
    {
        get => _attackDamage;
        set => _attackDamage = Mathf.Max(value);
    }

    public float MoveVelocity
    {
        get => _moveVelocity;
        set => _moveVelocity = value;
    }

    public int TokensDropped
    {
        get => _tokensDropped;
        set => _tokensDropped = value;
    }

    #region Modified
    
    public int ModifiedAttackDamage => Mathf.Max(AttackDamage + Game.Instance.GameModifiers.ExtraEnemyDamage, 1); 
    
    public float ModifiedMoveVelocity => MoveVelocity * Game.Instance.GameModifiers.ExtraScaleEnemyVelocity * (Game.Instance.Player.IsDead ? 0f : 1f);
    
    #endregion
}