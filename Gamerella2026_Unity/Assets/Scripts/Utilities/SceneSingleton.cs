using UnityEngine;

/// <summary>
/// Like a singleton, but one that expects itself to be destroyed and recreated multiple times.
/// There is still the restriction of a single instance at any one time, but there might be multiple instances over time.
/// </summary>
public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance != null)
                {
                    Debug.LogWarning($"Singleton `{nameof(T)}` assigned through FindFirstObjectByType");
                }
            }

            if (_instance == null)
            {
                Debug.LogWarning($"Singleton `{nameof(T)}` assigned through new GameObject creation");
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = $"Singleton ({typeof(T)})";
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
    }
}