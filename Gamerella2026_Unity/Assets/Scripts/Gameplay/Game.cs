using System;
using UnityEngine;
using static EndGameScreen;

public class Game : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController = null;

    [SerializeField]
    private EnemyManager _enemyManager = null;

    [SerializeField]
    private EnemySpawner _enemySpawner = null;
    
    [SerializeField]
    private GameModifiers _gameModifiers = null;
    
    [SerializeField]
    private Transform _lootboxesTransform = null;

    public float ElapsedTimeSeconds => Time.realtimeSinceStartup - _gameStartedSeconds;
    
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
    
    public PlayerController Player => _playerController;

    public EnemyManager EnemyManager => _enemyManager;

    public EnemySpawner EnemySpawner => _enemySpawner;
    
    public GameModifiers GameModifiers => _gameModifiers;
    
    public Transform LootboxesTransform => _lootboxesTransform;

    private float _gameStartedSeconds;

    private Action<EndGameData> _onEndGame;
    
    public void Init(Action<EndGameData> onEndGame)
    {
        _onEndGame = onEndGame;
    }

    public void EndGame()
    {
        Destroy(gameObject);
        _onEndGame?.Invoke(new EndGameData(ElapsedTimeSeconds));
    }
    
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
