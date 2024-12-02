using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T: Component, new()
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var gameObject = new GameObject { name = nameof(T) + " (Singleton)" };
                DontDestroyOnLoad(gameObject);

                _instance = gameObject.AddComponent<T>();
            }

            return _instance;
        }
    }
}
