using System;
using FMOD.Studio;
using FMODUnity;
using Gameplay;
using UnityEngine;
using static EndGameScreen;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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

    [SerializeField]
    private HitTextPool _hitTextPool = null;
    
    [SerializeField]
    private Transform _hitTestTransform = null;
    
    [SerializeField]
    private Casino _casino = null;
    
    [SerializeField] 
    private EventReference _soundtrackReference;
    
    [SerializeField] 
    private EventReference _ambienceReference;

    private EventInstance _soundtrackInstance;
    
    private EventInstance _ambienceInstance;
    
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

    public Casino Casino => _casino;
    
    public HitTextPool HitTextPool => _hitTextPool;

    public float ElapsedTimeSeconds => Time.realtimeSinceStartup - _gameStartedSeconds;
    
    public PlayerController Player => _playerController;

    public EnemyManager EnemyManager => _enemyManager;

    public EnemySpawner EnemySpawner => _enemySpawner;
    
    public GameModifiers GameModifiers => _gameModifiers;
    
    public Transform LootboxesTransform => _lootboxesTransform;
    
    public Transform HitTestTransform => _hitTestTransform;

    private float _gameStartedSeconds;

    private Action<EndGameData> _onEndGame;
    
    public void Init(Action<EndGameData> onEndGame)
    {
        _onEndGame = onEndGame;
    }

    public void EndGame()
    {
        _soundtrackInstance.stop(STOP_MODE.ALLOWFADEOUT);
        _soundtrackInstance.release();
        
        _ambienceInstance.stop(STOP_MODE.ALLOWFADEOUT);
        _ambienceInstance.release();
        
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
        
        _soundtrackInstance = RuntimeManager.CreateInstance(_soundtrackReference);
        _soundtrackInstance.start();
        
        _ambienceInstance = RuntimeManager.CreateInstance(_ambienceReference);
        _ambienceInstance.start();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }
    }
}
