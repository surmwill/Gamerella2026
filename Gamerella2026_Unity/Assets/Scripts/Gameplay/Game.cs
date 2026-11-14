using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController = null;

    [SerializeField]
    private EnemyManager _enemyManager = null;

    public float ElapsedTimeSeconds => Time.realtimeSinceStartup - _gameStartedSeconds;

    private float _gameStartedSeconds;
    
    public static Game Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<Game>();
            }

            if (_instance == null)
            {
                Debug.LogError($"Expected component of type {nameof(Game)}");
            }

            return _instance;
        }
    }
    
    public PlayerController PlayerController => _playerController;

    public EnemyManager EnemyManager => _enemyManager;
    
    private static Game _instance;
    
    private void Awake()
    {
        _instance = GetComponent<Game>();
        if (_instance == null)
        {
            Debug.LogError($"Expected component of type {nameof(Game)}");
        }
    }

    private void Start()
    {
        _gameStartedSeconds = Time.realtimeSinceStartup;
    }
}
