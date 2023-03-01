using UnityEngine;

public class ObjectPreserver : MonoBehaviour
{
    [field: SerializeField] public bool Enabled { get; private set; } = true;

    private void Awake()
    {
        if (Enabled) DontDestroyOnLoad(this);
    }
}
