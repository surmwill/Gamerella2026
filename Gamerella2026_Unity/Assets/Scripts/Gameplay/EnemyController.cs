using DG.Tweening;
using TMPro;
using UnityEngine;
using static EnemyConfigurationsSO;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyConfigurationsSO _enemyConfigurationsSO = null;
    
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    [SerializeField]
    private Rigidbody2D _enemyRigidbody = null;

    [SerializeField]
    private Collider2D _collider = null;

    [SerializeField]
    private Lootbox _lootboxPrefab = null;

    [SerializeField]
    private Transform _model = null;
    
    [SerializeField]
    private SkinnedMeshRenderer _enemyRenderer = null;

    [Header("Stats")]
    [SerializeField]
    [ReadOnly]
    private EnemyStats _enemyStats;

    public EnemyConfigurationsSO GlobalConfig => _enemyConfigurationsSO;
    
    public EnemyConfiguration LocalConfig { get; private set; }
    
    public EnemyStats Stats => _enemyStats;
    
    private PlayerController Player => Game.Instance.Player;
    
    public Collider2D Collider => _collider;
        
    private Sequence _knockedBackSequence = null;
    private Sequence _hitTestSequence = null;
    
    public void SpawnWithConfiguration(EnemyConfiguration enemyConfiguration)
    {
        LocalConfig = enemyConfiguration;
        
        _enemyStats.Init(enemyConfiguration.BaseStats);

        _enemyRenderer.material = enemyConfiguration.PrefabConfig.Material;
        
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
        _enemyRigidbody.linearVelocity = toPlayer * Stats.ModifiedMoveVelocity; 
    }

    private void FacePlayer()
    {
        _model.localScale = _model.localScale.WithX(Player.transform.position.x > transform.position.x ? 1f : -1f);
    }

    public void OnAttackedByPlayer(PlayerController player)
    {
        // Show the damage
        ShowHitText(player.Stats.AttackDamage.ToString());
        
        // Subtract the health
        Stats.Health = Mathf.Max(Stats.Health - player.Stats.AttackDamage, 0);
        if (Stats.Health <= 0)
        {
            Kill();
            return;
        }
        
        // Knockback if still alive
        ApplyKnockbackFromPlayer(player);
    }

    public void ShowHitText(string text)
    {
        _hitTestSequence?.Kill();
        
        TMP_Text hitText = Game.Instance.HitTextPool.FetchTextFromPool();
        hitText.transform.SetParent(Game.Instance.HitTestTransform);
        
        // Ensure it's rendered on top
        hitText.transform.position = transform.position.WithY(transform.position.y + GlobalConfig.HitTextOffsetY).WithZ(-0.003f);

        hitText.text = text;
        hitText.alpha = 1.0f;
        
        _hitTestSequence = DOTween.Sequence()
            .Append(hitText.DOFade(0f, GlobalConfig.HitTextDuration))
            .OnKill(() =>
            {
                if (Game.Instance != null)
                {
                    Game.Instance.HitTextPool.ReturnTextToPool(hitText);   
                }
            });
    }

    private void Kill()
    {
        Player.NumTokens += Stats.TokensDropped;
        
        _knockedBackSequence?.Kill();
        
        Game.Instance.EnemySpawner.CleanupEnemy(this);

        if (Random.value <= Lootbox.LootboxDropChance)
        {
            DOTween.Sequence().AppendInterval(1.5f).OnComplete(() =>
            {
                Lootbox lootbox = Instantiate(_lootboxPrefab, Game.Instance.LootboxesTransform);
                lootbox.transform.position = transform.position;
            });
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
        _enemyRigidbody.linearDamping = GlobalConfig.KnockbackLinearDampening;
        _enemyRigidbody.AddForce(rawDirection * player.Stats.KnockbackForce, ForceMode2D.Impulse);

        // Disable the collider while spinning out
        _collider.enabled = false;

        float baseSpins = player.Stats.KnockbackDuration * GlobalConfig.KnockbackSpinsPerSecond;
        float extraSpins = Random.value * 0.5f * (Random.value >= 0.5f ? 1f : -1f);
        
        // Switch back to using linear velocity
        _knockedBackSequence = DOTween.Sequence()
            .AppendInterval(player.Stats.KnockbackDuration)
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