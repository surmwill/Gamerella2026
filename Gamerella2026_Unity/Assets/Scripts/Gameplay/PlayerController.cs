using ProphetAR;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _velocityMultiplier = 1f;
    
    [SerializeField]
    private Rigidbody2D _playerRigidbody = null;

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
        _playerRigidbody.linearVelocity = _moveInput * _velocityMultiplier;
    }
}
