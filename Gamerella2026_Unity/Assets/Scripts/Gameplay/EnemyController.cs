using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    [SerializeField]
    private Rigidbody2D _enemyRigidbody = null;
    
    [Header("Stats")]
    [SerializeField]
    private float _baseScaleVelocity = 10f;
    
    [SerializeField]
    private int _baseAttackDamage = 10;

    [SerializeField]
    private float _baseAttacksPerSecond = 0.1f;

    public float ExternalScaleVelocity { get; set; } = 1f;
    
    private float ScaleVelocity => _baseScaleVelocity * ExternalScaleVelocity;
    
    private float AttackDamage => _baseAttackDamage + Game.Instance.GameModifiers.ExtraEnemyDamage;

    private PlayerController Player => Game.Instance.Player;
    
    public void OnSpawned()
    {
        FacePlayer();
    }

    private void FixedUpdate()
    {
        Vector3 toPlayer = (Player.transform.position - transform.position).normalized;
        _enemyRigidbody.linearVelocity = toPlayer * ScaleVelocity;
        
        FacePlayer();
    }

    private void FacePlayer()
    {
        _spriteRenderer.flipX = Player.transform.position.x > transform.position.x;
    }

    public void OnAttack()
    {
    }

    public void OnKilled()
    {
        Game.Instance.EnemySpawner.CleanupEnemy(this);
    }
}
