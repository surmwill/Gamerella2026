using UnityEngine;

/// <summary>
/// A single globally accessible instance of a MonoBehaviour that is not expected to be destroyed for the entirety
/// of the program (DontDestroyOnLoad)
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _isShuttingDown;

    public static T Instance
    {
        get
        {
            // Don't recreate an instance that's in the process of being destroyed
            if (_isShuttingDown)
            {
                Debug.LogWarning($"Singleton `{nameof(T)}` has been destroyed, returning null.");
                return null;
            }

            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance != null)
                {
                    Debug.LogWarning($"Singleton `{nameof(T)}` assigned through FindFirstObjectByType");
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }

            if (_instance == null)
            {
                Debug.LogWarning($"Singleton `{nameof(T)}` assigned through new GameObject creation");
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = $"Singleton ({typeof(T)})";
                DontDestroyOnLoad(singletonObject);
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // Ensure a single instance is managed on a single GameObject
        if (_instance != null && _instance.gameObject != gameObject)
        {
            Debug.LogError($"Duplicate instance of `{nameof(T)}`, destroying this instance/GameObject");
            Destroy(gameObject);
            return;
        }

        // This GameObject manages the instance
        _instance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _isShuttingDown = true;
        }
    }
}