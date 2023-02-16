using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (!s_instance) s_instance = new GameObject(typeof(T).Name).AddComponent<T>();
            return s_instance;
        }
    }

    private static T s_instance;

    private void Awake()
    {
        if (s_instance && s_instance != this)
            Destroy(gameObject);
        else if (!s_instance)
            s_instance = this as T;
    }
}
