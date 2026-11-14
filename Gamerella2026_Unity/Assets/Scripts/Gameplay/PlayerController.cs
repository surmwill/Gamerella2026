using ProphetAR;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _baseScaleVelocity = 20f;
    
    [SerializeField]
    private Rigidbody2D _playerRigidbody = null;

    public float ExternalScaleVelocity { get; set; } = 1f;
    
    private float TotalScaleVelocity => _baseScaleVelocity * ExternalScaleVelocity; 
    
    private Vector2 _moveInput;
    
    private void Update()
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

    private void FixedUpdate()
    {
        _playerRigidbody.linearVelocity = _moveInput * TotalScaleVelocity;
    }
}
