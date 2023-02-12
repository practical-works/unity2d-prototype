using UnityEngine;

public class Switch : MonoBehaviour
{
    public Door[] Doors;
    public string UserTag = "";
    public bool IsSticky;
    [Header("Animator")]
    public bool UseAnimator = true;
    public string AnimBoolParamName = "On";

    [Header("Debug")]
    [SerializeField] private bool _isOn;
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    private Color _gizmosColor;

    public bool IsOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            if (UseAnimator && _animator) _animator.SetBool(AnimBoolParamName, _isOn);
            foreach (Door door in Doors) if (door) door.IsOpen = _isOn;
        }
    }

    private void Awake() => _animator = GetComponent<Animator>();

    private void OnTriggerEnter2D(Collider2D collider) => HandleOnTrigger(collider, on: true);

    private void OnTriggerExit2D(Collider2D collider) => HandleOnTrigger(collider, on: false);

    private void OnValidate() => IsOn = _isOn;

    private void HandleOnTrigger(Collider2D collider, bool on)
    {
        if (IsSticky && !on) return;
        if (!string.IsNullOrEmpty(UserTag) && !collider.CompareTag(UserTag)) return;
        IsOn = on;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor == default ?
            (TryGetComponent(out SpriteRenderer spriteRenderer) ? spriteRenderer.color : Color.gray) : _gizmosColor;
        foreach (Door door in Doors) if (door) Gizmos.DrawLine(transform.position, door.transform.position);
        if (_boxCollider2D || (!_boxCollider2D && TryGetComponent<BoxCollider2D>(out _boxCollider2D)))
            Gizmos.DrawWireCube(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size);
    }
}
