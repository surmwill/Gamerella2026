using DG.Tweening;
using UnityEngine;
using static EnemyConfigurationsSO;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    [SerializeField]
    private Rigidbody2D _enemyRigidbody = null;

    [SerializeField]
    private Collider2D _collider = null;
    
    [SerializeField]
    private float _knockbackLinearDampening = 0.15f;

    [SerializeField]
    [ReadOnly]
    private float _health;
    
    [Header("Stats")]
    [SerializeField]
    private EnemyBaseStats _baseStats = null;

    public float Health
    {
        get => _health; 
        private set => _health = value;
    }

    public float AttackDamage => _baseStats.BaseAttackDamage + Game.Instance.GameModifiers.ExtraEnemyDamage;
    
    public Collider2D Collider => _collider;
    
    public float ExternalScaleVelocity { get; set; } = 1f;
    
    private float ScaleVelocity => _baseStats.BaseScaleVelocity * ExternalScaleVelocity;

    private PlayerController Player => Game.Instance.Player;
        
    private Sequence _knockedBackSequence = null;
    
    public void SpawnWithConfiguration(EnemyConfiguration enemyConfiguration)
    {
        _baseStats = enemyConfiguration.BaseStats;
        Health = enemyConfiguration.BaseStats.BaseHealth;
        
        FacePlayer();
    }

    private void FixedUpdate()
    {
        if (!_knockedBackSequence?.IsActive() ?? true)
        {
            DoMovement();
        }
        
        FacePlayer();
    }

    private void DoMovement()
    {
        Vector3 toPlayer = (Player.transform.position - transform.position).normalized;
        _enemyRigidbody.linearVelocity = toPlayer * ScaleVelocity; 
    }

    private void FacePlayer()
    {
        _spriteRenderer.flipX = Player.transform.position.x > transform.position.x;
    }

    public void OnAttacked(PlayerController player)
    {
        Health = Mathf.Max(Health - player.AttackDamage, 0f);
        if (Health <= 0f)
        {
            Kill();
            return;
        }
        
        ApplyKnockback(player.transform.position, player.KnockbackForce, player.KnockbackDuration);
    }

    private void Kill()
    {
        Player.NumTokens += _baseStats.TokensDropped;
        
        _knockedBackSequence?.Kill();
        Game.Instance.EnemySpawner.CleanupEnemy(this);
    }
    
    /// <summary>
    /// Applies knockback via physics impulse, respecting walls and map bounds.
    /// </summary>
    private void ApplyKnockback(Vector3 damageSourcePosition, float knockbackForce, float knockbackDuration)
    {
        Vector3 currentPos = transform.position;
        Vector3 rawDirection = (currentPos - damageSourcePosition).normalized;
        
        // Switch to using force
        _enemyRigidbody.linearVelocity = Vector3.zero;
        _enemyRigidbody.linearDamping = _knockbackLinearDampening;
        _enemyRigidbody.AddForce(rawDirection * knockbackForce, ForceMode2D.Impulse);
        
        // Switch back to using linear velocity
        _knockedBackSequence = DOTween.Sequence()
            .AppendInterval(knockbackDuration)
            .OnKill(() => _enemyRigidbody.linearDamping = 0f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Game.Instance.Player.gameObject)
        {
            Game.Instance.Player.OnAttacked(this);
        }
    }
}