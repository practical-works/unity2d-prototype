using Sirenix.OdinInspector;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;

    public static T Instance
    {
        get
        {
            if (!s_instance) s_instance = FindObjectOfType<T>();
            if (!s_instance) s_instance = new GameObject(typeof(T).Name).AddComponent<T>();
            return s_instance;
        }
    }

    internal virtual void Awake() => PreventMultipleInstances();

    internal virtual void OnValidate() => PreventMultipleInstances();

    private void PreventMultipleInstances()
    {
        // Todo Fix: Object prefab + in-scene thinks it got 2 instances by that & triggers the destruction
        if (s_instance && s_instance != this)
        {
            if (Application.isPlaying) Destroy(this);
            else UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(this);
            Debug.LogWarning($"Cannot create multiple instances of {s_instance.GetType()} component.\nAn instance is already attached to " +
                $"\"{s_instance.name}\" object. Therefore, created duplicate instance attached to \"{gameObject.name}\" object is destroyed.");
        }
        else if (!s_instance)
            s_instance = this as T;
    }
}
