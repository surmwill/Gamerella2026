using System;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController = null;

    [SerializeField]
    private EnemyFactory _enemyFactor = null;
    
    private static Game _instance;
    
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

    public EnemyFactory EnemyFactory => _enemyFactor;
    
    private void Awake()
    {
        _instance = GetComponent<Game>();
        if (_instance == null)
        {
            Debug.LogError($"Expected component of type {nameof(Game)}");
        }
    }
    
    
}
