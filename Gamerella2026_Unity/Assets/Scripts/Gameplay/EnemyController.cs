using DG.Tweening;
using ProphetAR;
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
    private Lootbox _lootboxPrefab = null;
    
    [SerializeField]
    private float _knockbackLinearDampening = 0.15f;

    [SerializeField]
    private float _knockbackSpinsPerSecond = 1f;
    
    [Header("Stats")]
    [SerializeField]
    private float _health;
    
    [SerializeField]
    private EnemyBaseStats _baseStats = null;

    public float Health
    {
        get => _health; 
        private set => _health = value;
    }

    public int AttackDamage => _baseStats.BaseAttackDamage + Game.Instance.GameModifiers.ExtraEnemyDamage;
    
    public Collider2D Collider => _collider;
    
    private float ScaleVelocity => _baseStats.BaseScaleVelocity * Game.Instance.GameModifiers.ExtraScaleEnemyVelocity;

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
        // If we're not knocked back do movement like normal
        if (!_knockedBackSequence?.IsActive() ?? true)
        {
            DoMovement();
            FacePlayer();
        }
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

    public void OnAttackedByPlayer(PlayerController player)
    {
        Health = Mathf.Max(Health - player.AttackDamage, 0f);
        if (Health <= 0f)
        {
            Kill();
            return;
        }
        
        ApplyKnockbackFromPlayer(player);
    }

    private void Kill()
    {
        Player.NumTokens += _baseStats.TokensDropped;
        
        _knockedBackSequence?.Kill();
        Game.Instance.EnemySpawner.CleanupEnemy(this);

        if (Random.value < Lootbox.LootboxDropChance)
        {
            Lootbox lootbox = Instantiate(_lootboxPrefab, Game.Instance.LootboxesTransform);
            lootbox.transform.position = transform.position;
        }
    }
    
    /// <summary>
    /// Applies knockback via physics impulse, respecting walls and map bounds.
    /// </summary>
    public void ApplyKnockbackFromPlayer(PlayerController player)
    {
        _knockedBackSequence?.Kill();
        
        Vector3 currentPos = transform.position;
        Vector3 rawDirection = (currentPos - player.transform.position).normalized;
        
        // Switch to using force
        _enemyRigidbody.linearVelocity = Vector3.zero;
        _enemyRigidbody.linearDamping = _knockbackLinearDampening;
        _enemyRigidbody.AddForce(rawDirection * player.KnockbackForce, ForceMode2D.Impulse);

        // Disable the collider while spinning out
        _collider.enabled = false;

        float baseSpins = player.KnockbackDuration * _knockbackSpinsPerSecond;
        float extraSpins = Random.value * 0.5f * (Random.value >= 0.5f ? 1f : -1f);
        
        
        // Switch back to using linear velocity
        _knockedBackSequence = DOTween.Sequence()
            .AppendInterval(player.KnockbackDuration)
            .Join(transform.DOLocalRotate(new Vector3(0, 0, 360f * baseSpins), baseSpins + extraSpins, RotateMode.FastBeyond360).SetEase(Ease.Linear))
            .OnKill(() =>
            {
                _enemyRigidbody.linearDamping = 0f;
                _collider.enabled = true;
                transform.localEulerAngles = transform.localEulerAngles.WithZ(0f);
            });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Game.Instance.Player.gameObject)
        {
            Game.Instance.Player.OnAttackedByEnemy(this);
        }
    }
}