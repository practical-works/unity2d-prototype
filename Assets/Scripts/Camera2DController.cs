using UnityEngine;

public class Camera2DController : MonoBehaviour
{
    public Transform TargetTransform;
    public bool FollowTarget = true;
    public Vector2 Offset;
    [Header("Easing")]
    public bool Easing = true;
    public Easings.Methods EasingMethod = 0;
    [Min(0f)] public float EasingSeconds = 3f;

    private Camera _camera;
    private Vector3 _initialPosition;
    private float _elapsedTime;

    public Vector3 TargetPosition => 
        new Vector3(TargetTransform.position.x, TargetTransform.position.y, transform.position.z) + (Vector3)Offset;
    public bool TargetPositionIsDifferent => TargetPosition != transform.position; 

    private void LateUpdate()
    {
        if (FollowTarget && TargetTransform is not null && TargetPositionIsDifferent)
        {
            if (Easing) FollowTargetWithEasing();
            else FollowTargetDirectly();
        }
    }

    private void FollowTargetWithEasing()
    {
        if (_initialPosition == default) _initialPosition = transform.position;
        Debug.Log($"Elapsed Time : {_elapsedTime}");
        _elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(_initialPosition, TargetPosition, Easings.Perform(_elapsedTime / EasingSeconds, EasingMethod));
        if (_elapsedTime < EasingSeconds) 
            _elapsedTime = Easings.Perform(_elapsedTime + Time.deltaTime, EasingMethod);
        else
        {
            _elapsedTime = 0f;
            _initialPosition = default; 
        }
    }

    private void FollowTargetDirectly()
    {
        transform.position = new Vector3(TargetTransform.position.x, TargetTransform.position.y, transform.position.z);
    }

    [ContextMenu("Reset Following")]
    private void ResetFollowing()
    {
        FollowTarget = false;
        _elapsedTime = 0f;
        _initialPosition = default;
        transform.position = new Vector3(0f, 0f, transform.position.z);
        Debug.Log($"[Reset] Elapsed Time : {_elapsedTime}");
    }

    private void OnDrawGizmos()
    {
        if (TargetTransform)
        {
            Gizmos.DrawLine(transform.position, TargetPosition);
            Gizmos.DrawLine(TargetPosition, TargetTransform.position);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(TargetPosition, 0.3f * Vector3.one);
            if (!_camera) _camera = GetComponent<Camera>();
            Gizmos.DrawWireCube(TargetPosition, new Vector3(2f * _camera.orthographicSize * _camera.aspect, 2f * _camera.orthographicSize));
        }
    }
}
