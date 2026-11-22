using System;
using DG.Tweening;
using FMODUnity;
using Gameplay;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _playerRigidbody = null;

    [SerializeField]
    private CircleCollider2D _attackArea = null;

    [SerializeField]
    private Transform _model = null;

    [SerializeField]
    private Animator _playerAnimator = null;

    [SerializeField]
    private EventReference _onAttackSoundReference;
    
    [SerializeField]
    private EventReference _onDeathSoundReference;
    
    [SerializeField] 
    private EventReference _footstepSoundReference;
    
    [SerializeField]
    private PlayerConfigurationSO _playerConfigurationSO = null;
    
    [Header("Stats")]
    [SerializeField]
    private PlayerStats _playerStats = null;
    
    public event Action<int> OnPlayerTokensUpdated;

    public PlayerStats Stats => _playerStats;
    
    public PlayerConfigurationSO Config => _playerConfigurationSO;
    
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
    
    public bool IsDead { get; set; } = false;

    public bool CanAttack => Game.Instance.ElapsedTimeSeconds - _timeOfLastAttack >= Stats.ModifiedAttackCooldownSeconds;

    private const int MaxEnemiesDetectedInAttackRange = 10; 
    
    private static readonly int AttackStateHash = Animator.StringToHash("Attack");
    private static readonly int DeathStateHash = Animator.StringToHash("Dead-GameOver");
    
    private readonly Collider2D[] _enemiesInAttackRange = new Collider2D[MaxEnemiesDetectedInAttackRange];

    private int _numTokens = 50;
    
    private float _timeOfLastAttack;
    
    private Vector2 _moveInput;

    private void Awake()
    {
        _playerStats.Init(_playerConfigurationSO);
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
        
        _playerRigidbody.linearVelocity = _moveInput * _playerStats.MoveVelocity;

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
        _playerStats.Health = Math.Max(_playerStats.Health - enemy.AttackDamage, 0);
        if (_playerStats.Health <= 0)
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
        RuntimeManager.PlayOneShot(_onAttackSoundReference);
        
        enemy.OnAttackedByPlayer(this);
        _timeOfLastAttack = Game.Instance.ElapsedTimeSeconds;
    }

    private void OnDie()
    {
        RuntimeManager.PlayOneShot(_onDeathSoundReference);
        
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        IsDead = true;
        
        _playerAnimator.Play(DeathStateHash);
        DOTween.Sequence().AppendInterval(2.0f).AppendCallback(Game.Instance.EndGame);
    }
}
