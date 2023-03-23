using Sirenix.OdinInspector;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;

    [ShowInInspector]
    public static T Instance
    {
        get
        {
            if (!s_instance)
            {
                s_instance = FindObjectOfType<T>();
                if (s_instance) s_instance.name = $"${s_instance.name}";
                else s_instance = new GameObject($"${typeof(T).Name}").AddComponent<T>();
            }
            return s_instance;
        }
    }

    //[ShowInInspector] private bool IsStagedPrefab => !!PrefabStageUtility.GetPrefabStage(gameObject);

    internal virtual void Awake() => PreventMultipleInstancesRuntime();

    internal virtual void OnValidate() => PreventMultipleInstancesRuntime();

    private void PreventMultipleInstancesRuntime()
    {
        // Todo Fix: Object prefab + in-scene thinks it got 2 instances by that & triggers the destruction
        if (s_instance && s_instance != this)
        {
            if (Application.isPlaying) Destroy(this);
            else UnityEditor.EditorApplication.delayCall += () => DestroyImmediate(this);
            Debug.LogWarning($"Cannot create multiple instances of {s_instance.GetType()} component.\nAn instance is already attached to " +
                $"\"{s_instance.name}\" object. Therefore, duplicate instance attached to \"{gameObject.name}\" object is destroyed.");
        }
        else if (!s_instance)
            s_instance = this as T;
    }

    //private void PreventMultipleInstancesEditor()
    //{
    //    PreventMultipleInstancesRuntime();
    //}
}
