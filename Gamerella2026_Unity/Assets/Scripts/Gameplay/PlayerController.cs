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
    
    // Note: multiplicative
    private float ScaleVelocity => _baseScaleVelocity * Game.Instance.GameModifiers.ExtraScalePlayerVelocity;

    private float AttackDamage => _baseAttackDamage + Game.Instance.GameModifiers.ExtraPlayerDamage;

    private float AttacksPerSecond => _baseAttacksPerSecond + Game.Instance.GameModifiers.ExtraPlayerAttackSpeed;
    
    private Vector2 _moveInput;
    
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

    private void AttackClosestEnemy()
    {
        
    }

    private void FixedUpdate()
    {
        _playerRigidbody.linearVelocity = _moveInput * ScaleVelocity;
    }
}
