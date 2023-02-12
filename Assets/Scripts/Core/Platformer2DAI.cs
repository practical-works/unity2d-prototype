using System;
using UnityEngine;

[RequireComponent(typeof(Platformer2D))]
public class Platformer2DAI : MonoBehaviour
{
    [Header("Movement")]
    [Range(-1f, 1f)] public int MovementDirection = 1;
    public LayerMask ObstacleLayers = 1 << 3;
    [Header("Wall Detector")]
    public bool WallDetectorEnabled = true;
    public DetectorLine WallDetectorLine;
    [Header("Floor Detector")]
    public bool FloorDetectorEnabled = true;
    public DetectorLine FloorDetectorLine;

    private Platformer2D _platformer2D;
    private RaycastHit2D _wallDetection;
    private RaycastHit2D _floorDetection;

    private void Awake()
    {
        _platformer2D = GetComponent<Platformer2D>();
        if (!WallDetectorLine.Transform) WallDetectorLine.Transform = transform;
        if (!FloorDetectorLine.Transform) FloorDetectorLine.Transform = transform;
    }

    private void FixedUpdate()
    {
        if (!_platformer2D.IsFalling) _platformer2D.Move(MovementDirection);
        if (WallDetectorEnabled) _wallDetection = WallDetectorLine.Detect2D(ObstacleLayers);
        if (FloorDetectorEnabled) _floorDetection = FloorDetectorLine.Detect2D(ObstacleLayers);
        if ((WallDetectorEnabled && _wallDetection) || (FloorDetectorEnabled && !_floorDetection)) 
            MovementDirection = -MovementDirection;
    }

    #region Editor
    private void Reset()
    {
        WallDetectorLine = new DetectorLine(transform, from: new(0f, 0.2f, 0f), to: new(1f, 0.2f, 0f));
        FloorDetectorLine = new DetectorLine(transform, from: new(0f, 0.2f, 0f), to: new(2f, 0f, 0f));
    }

    private void OnDrawGizmos()
    {
        if (WallDetectorEnabled) WallDetectorLine.DrawGizmo(_wallDetection ? Color.green : Color.red);
        if (FloorDetectorEnabled) FloorDetectorLine.DrawGizmo(_floorDetection ? Color.green : Color.red);
    }
    #endregion
}

[Serializable]
public class DetectorLine
{
    public Transform Transform;
    public Vector3 LocalStart;
    public Vector3 LocalEnd;

    public Vector3 Start => Transform.TransformPoint(LocalStart);
    public Vector3 End => Transform.TransformPoint(LocalEnd);

    public DetectorLine(Transform transform, Vector3 from, Vector3 to)
    {
        Transform = transform;
        LocalStart = from;
        LocalEnd = to;
    }

    public RaycastHit2D Detect2D(LayerMask layerMask)
    {
        return Physics2D.Linecast(Start, End, layerMask);
    }

    public void DrawGizmo(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(Start, End);
    }
}
