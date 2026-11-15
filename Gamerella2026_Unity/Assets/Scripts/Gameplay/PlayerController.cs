using System;
using DG.Tweening;
using ProphetAR;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _playerRigidbody = null;

    [SerializeField]
    private CircleCollider2D _attackArea = null;
    
    [Header("Stats")]
    [ReadOnly]
    [SerializeField]
    private int _health = 100;
    
    [SerializeField]
    private float _baseScaleVelocity = 20f;
    
    [SerializeField]
    private int _baseAttackDamage = 10;

    [SerializeField]
    private float _baseAttacksPerSecond = 0.1f;

    [SerializeField]
    private int _baseHealth = 100;

    [SerializeField]
    private float _knockbackForce = 10f;
    
    [SerializeField]
    private float _knockbackDuration = 0.25f;

    [SerializeField]
    private Transform _model = null;

    [SerializeField]
    private Animator _playerAnimator = null;
    
    public event Action<int> OnPlayerTokensUpdated;
    
    public int NumTokens 
    { 
        get => _numTokens;
        set
        {
            if (value != _numTokens)
            {
                _numTokens = value;
                OnPlayerTokensUpdated?.Invoke(NumTokens);   
            }
        }
    }

    public int Health
    {
        get => _health; 
        private set => _health = value;
    }
    
    public bool IsDead { get; set; } = false;

    public int BaseHealth => _baseHealth;
    
    public float AttackDamage => Mathf.Max(_baseAttackDamage + Game.Instance.GameModifiers.ExtraPlayerDamage, 1f);
    
    public float KnockbackForce => _knockbackForce;
    
    public float KnockbackDuration => _knockbackDuration;

    public bool CanAttack => Game.Instance.ElapsedTimeSeconds - _timeOfLastAttack >= AttackCooldownSeconds;
    
    // Note: multiplicative
    private float ScaleVelocity => _baseScaleVelocity * Game.Instance.GameModifiers.ExtraScalePlayerVelocity;
    
    private float AttackCooldownSeconds => 1f / (_baseAttacksPerSecond + Game.Instance.GameModifiers.ExtraPlayerAttacksPerSecond);

    private const int MaxEnemiesDetectedInAttackRange = 10; 
    
    private static readonly int AttackStateHash = Animator.StringToHash("Attack");
    private static readonly int DeathStateHash = Animator.StringToHash("Dead-GameOver");
    
    private readonly Collider2D[] _enemiesInAttackRange = new Collider2D[MaxEnemiesDetectedInAttackRange];

    private int _numTokens;
    
    private float _timeOfLastAttack;
    
    private Vector2 _moveInput;

    private void Awake()
    {
        Health = _baseHealth;
    }
    
    private void FixedUpdate()
    {
        if (IsDead)
        {
            return;
        }
        
        if (CanAttack)
        {
            TryAttackClosestEnemy();
        }
        
        _playerRigidbody.linearVelocity = _moveInput * ScaleVelocity;

        // Face correction direction
        if (Mathf.Abs(_moveInput.x) > 0)
        {
            _model.localScale = _model.localScale.WithX(_moveInput.x >= 0f ? 1f : -1f);
        }
    }

    private void Update()
    {
        if (!IsDead)
        {
            HandleMovement();   
        }
    }

    private void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
        }
        
        _moveInput = new Vector2(moveX, moveY).normalized;
    }

    public void OnAttackedByEnemy(EnemyController enemy)
    {
        if (IsDead)
        {
            return;
        }
        
        // Deflect if we can attack
        if (CanAttack)
        {
            AttackEnemy(enemy);
            return;
        }
        
        // Otherwise we take the hit
        Health = Math.Max(Health - enemy.AttackDamage, 0);
        if (Health <= 0)
        {
            OnDie();
        }
        
        enemy.ApplyKnockbackFromPlayer(this);
        enemy.ShowHitText("Knockback Only");
    }

    private bool TryAttackClosestEnemy()
    {
        int numEnemies = _attackArea.GetOverlappingColliders(_enemiesInAttackRange, LayerMask.GetMask(LayerNames.Enemy));
        
        EnemyController closestEnemy = null;
        float? closestSqrDistance = null;
        
        for (int i = 0; i < numEnemies; i++)
        {
            if (!Game.Instance.EnemyManager.EnemiesByCollider.TryGetValue(_enemiesInAttackRange[i], out EnemyController enemy))
            {
                Debug.LogWarning("Collision with unregistered enemy");
                continue;
            }

            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;
            if (!closestSqrDistance.HasValue || sqrDistance < closestSqrDistance.Value)
            {
                closestSqrDistance = sqrDistance;
                closestEnemy = enemy;
            }
        }

        if (closestSqrDistance.HasValue)
        {
            AttackEnemy(closestEnemy);
            return true;
        }

        return false;
    }

    private void AttackEnemy(EnemyController enemy)
    {
        _playerAnimator.Play(AttackStateHash);
        
        enemy.OnAttackedByPlayer(this);
        _timeOfLastAttack = Game.Instance.ElapsedTimeSeconds;
    }

    private void OnDie()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        IsDead = true;
        
        _playerAnimator.Play(DeathStateHash);
        DOTween.Sequence().AppendInterval(2.0f).AppendCallback(Game.Instance.EndGame);
    }
}
