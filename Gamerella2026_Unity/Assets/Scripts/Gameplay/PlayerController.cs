using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _playerRigidbody = null;
    
    [Header("Stats")]
    [SerializeField]
    private float _baseScaleVelocity = 20f;
    
    [SerializeField]
    private int _baseAttackDamage = 10;

    [SerializeField]
    private float _baseAttacksPerSecond = 0.1f;

    [SerializeField]
    private float _baseHealth = 100f;
    
    public float Health { get; private set; }
    
    public bool CanAttack => Game.Instance.ElapsedTimeSeconds - _timeSinceLastAttack >= AttacksPerSecond;
    
    // Note: multiplicative
    private float ScaleVelocity => _baseScaleVelocity * Game.Instance.GameModifiers.ExtraScalePlayerVelocity;

    private float AttackDamage => _baseAttackDamage + Game.Instance.GameModifiers.ExtraPlayerDamage;

    private float AttacksPerSecond => _baseAttacksPerSecond + Game.Instance.GameModifiers.ExtraPlayerAttackSpeed;
    
    private float _timeSinceLastAttack = 0f;
    
    private Vector2 _moveInput;

    private void Awake()
    {
        Health = _baseHealth;
    }
    
    private void FixedUpdate()
    {
        _playerRigidbody.linearVelocity = _moveInput * ScaleVelocity;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }
        
        _moveInput = new Vector2(moveX, moveY).normalized;
    }

    public void TakeDamage(int damage)
    {
        Health = Mathf.Max(Health - damage, 0);
        if (Health <= 0)
        {
            OnDie();
        }
    }

    private void OnDie()
    {
        // Animation?
        // End game?
    }

    private void AttackClosestEnemy()
    {
        
    }

    private void AttackEnemy(EnemyController enemy)
    {
        
    }
}
