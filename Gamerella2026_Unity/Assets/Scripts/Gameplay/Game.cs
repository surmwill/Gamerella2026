using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController = null;
    
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
    
    private void Awake()
    {
        _instance = GetComponent<Game>();
        if (_instance == null)
        {
            Debug.LogError($"Expected component of type {nameof(Game)}");
        }
    }
    
    
}
