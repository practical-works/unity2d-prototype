using UnityEngine;

[RequireComponent(typeof(Platformer2D))]
public class Platformer2DController : MonoBehaviour
{
    [Header("Controls")]
    public string MovementButtonName = "Horizontal";
    public string JumpButtonName = "Jump";

    private Platformer2D _platformer2D;
    private float _horizontalAxis;
    private bool _jumpButtonPressed;

    private void Awake() => _platformer2D = GetComponent<Platformer2D>();

    private void Update()
    {
        _horizontalAxis = Input.GetAxis(MovementButtonName);
        if (Input.GetButtonDown(JumpButtonName)) _jumpButtonPressed = true;
    }

    private void FixedUpdate() => Move();

    private void Move()
    {
        _platformer2D.Move(_horizontalAxis);
        if (_jumpButtonPressed)
        {
            _platformer2D.Jump();
            _jumpButtonPressed = false;
        }
    }
}
