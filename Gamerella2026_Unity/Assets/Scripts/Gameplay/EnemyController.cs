using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    [SerializeField]
    private Rigidbody2D _enemyRigidbody = null;
    
    [SerializeField]
    private float _baseScaleVelocity = 10f;

    public float ExternalScaleVelocity { get; set; } = 1f;
    
    private float TotalScaleVelocity => _baseScaleVelocity * ExternalScaleVelocity;

    private PlayerController Player => Game.Instance.PlayerController;
    
    public void OnSpawned()
    {
        FacePlayer();
    }

    private void FixedUpdate()
    {
        Vector3 toPlayer = (Player.transform.position - transform.position).normalized;
        _enemyRigidbody.linearVelocity = toPlayer * TotalScaleVelocity;
        
        FacePlayer();
    }

    private void FacePlayer()
    {
        _spriteRenderer.flipX = Player.transform.position.x > transform.position.x;
    }

    public void KillEnemy()
    {
    }
}
