using UnityEngine;

[ExecuteAlways]
public class WallController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _floorSpriteRenderer = null;

    [SerializeField]
    private SpriteRenderer _leftWall = null;
    
    [SerializeField]
    private SpriteRenderer _rightWall = null;
    
    [SerializeField]
    private SpriteRenderer _topWall = null;
    
    [SerializeField]
    private SpriteRenderer _botWall = null;
    
    [SerializeField]
    private SpriteRenderer _wallPrefab = null;

    private void Update()
    {
        if (Application.isPlaying)
        {
            return;
        }

        Vector2 _floorSize = _floorSpriteRenderer.size;

        if (_leftWall != null)
        {
            _leftWall.transform.position = new Vector3(-_floorSize.x * 0.5f - _leftWall.size.x * 0.5f, 0f, 0f);
        }
        
        if (_rightWall != null)
        {
            _rightWall.transform.position = new Vector3(_floorSize.x * 0.5f + _leftWall.size.x * 0.5f, 0f, 0f);
        }

        if (_topWall != null)
        {
            _topWall.transform.position = new Vector3(0f, _floorSize.y * 0.5f + _topWall.size.y * 0.5f, 0f);
        }
        
        if (_botWall != null)
        {
            _botWall.transform.position = new Vector3(0f, -_floorSize.y * 0.5f - _botWall.size.y * 0.5f, 0f);
        }
    }
}
