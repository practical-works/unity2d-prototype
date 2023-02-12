using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Animator")]
    public string AnimBoolParamName = "Open";
    [Min(0f)] public float SecondsBeforeOpen = 0f;
    [Min(0f)] public float SecondsBeforeClose = 0f;

    [Header("Debug")]
    [SerializeField] private bool _isOpen;
    private Animator _animator;
    private IEnumerator _handleCoroutine;

    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            _isOpen = value;
            if (!_animator) return;
            if (_handleCoroutine != null) StopCoroutine(_handleCoroutine);
            _handleCoroutine = Handle(value);
            StartCoroutine(_handleCoroutine);
        }
    }

    private void Awake() => _animator = GetComponent<Animator>();

    private void OnValidate() => IsOpen = _isOpen;

    private IEnumerator Handle(bool open)
    {
        yield return new WaitForSeconds(open ? SecondsBeforeOpen : SecondsBeforeClose);
        _animator.SetBool(AnimBoolParamName, _isOpen = open);
    }
}
