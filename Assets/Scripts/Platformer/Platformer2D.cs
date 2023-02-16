using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Platformer2D : MonoBehaviour
{
    [Header("Movement")]
    [Range(0f, 50f)] public float MovementSpeed = 10f;
    [Range(0f, 1000f)] public float JumpStrength = 300f;

    [Header("Physics")]
    public Vector2 MaxVelocity = new(10f, 30f);
    public Vector2 ApproximateZeroVelocity = new(0.01f, 0.01f);

    [Header("Animator")]
    public string MovingAnimBoolParamName = "Moving";
    public string JumpingAnimBoolParamName = "Jumping";
    public string FallingAnimBoolParamName = "Falling";

    private Rigidbody2D _rigidBody2D;
    private Animator _animator;
    private float _originalDrag;

    public bool IsMoving => Mathf.Abs(_rigidBody2D.velocity.x) > ApproximateZeroVelocity.x;
    public bool IsJumping => _rigidBody2D.velocity.y > ApproximateZeroVelocity.y;
    public bool IsFalling => _rigidBody2D.velocity.y < -ApproximateZeroVelocity.y;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _originalDrag = _rigidBody2D.drag;
    }

    private void FixedUpdate()
    {
        _rigidBody2D.drag = IsFalling ? 0f : _originalDrag;
        Animate();
    }

    private void Animate()
    {
        _animator.SetBool(MovingAnimBoolParamName, IsMoving);
        _animator.SetBool(JumpingAnimBoolParamName, IsJumping);
        _animator.SetBool(FallingAnimBoolParamName, IsFalling);
    }

    public void FlipXScale(float direction)
    {
        if (direction == 0f) return;
        direction = Mathf.Sign(direction);
        float absLocalScaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(direction * absLocalScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Move(float direction, bool autoFlipXScale = true)
    {
        if (direction == 0f) return;
        if (Mathf.Abs(_rigidBody2D.velocity.x) >= MaxVelocity.x) return;
        direction = Mathf.Clamp(direction, -1f, 1f);
        _rigidBody2D.AddForce(direction * MovementSpeed * Vector2.right);
        if (autoFlipXScale) FlipXScale(direction);
    }

    public void Jump()
    {
        if (IsJumping || IsFalling) return;
        if (Mathf.Abs(_rigidBody2D.velocity.y) >= MaxVelocity.y) return;
        _rigidBody2D.AddForce(JumpStrength * Vector2.up);
    }
}
