using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Platformer2D))]
public class Platformer2DAI : MonoBehaviour
{
    [Header("Movement")]
    [Range(-1f, 1f)] public int MovementDirection = 1;
    public LayerMask ObstacleLayers = 1 << 3;
    public LayerMask ActorLayers = 1 << 7;
    [Header("Wall Detector")]
    public bool WallDetectorEnabled = true;
    public DetectorLine WallDetectorLine;
    [Header("Floor Detector")]
    public bool FloorDetectorEnabled = true;
    public DetectorLine FloorDetectorLine;
    [Header("Player Detector")]
    public bool ActorDetectorEnabled = true;
    public float ActorSpottingSeconds = 10f;
    public DetectorLine ActorDetectorLine;
    [Header("Projectile")]
    public GameObject Projectile;
    public float ProjectileForce = 10f;

    private Platformer2D _platformer2D;
    private RaycastHit2D _wallDetection;
    private RaycastHit2D _floorDetection;
    private RaycastHit2D _actorDetection;
    private float _actorSpottingStartTime;

    public bool SpottingActor => _actorDetection && Time.time < _actorSpottingStartTime + ActorSpottingSeconds;

    private void Awake()
    {
        _platformer2D = GetComponent<Platformer2D>();
        if (!WallDetectorLine.Transform) WallDetectorLine.Transform = transform;
        if (!FloorDetectorLine.Transform) FloorDetectorLine.Transform = transform;
    }

    private void FixedUpdate()
    {
        if (!_platformer2D.IsFalling) _platformer2D.Move(MovementDirection);
        Detectwall();
        DetectFloor();
        DetectActor();
    }

    private void Detectwall()
    {
        if (!WallDetectorEnabled) return;
        _wallDetection = WallDetectorLine.Detect2D(ObstacleLayers);
        if (_wallDetection) MovementDirection = -MovementDirection;
    }

    private void DetectFloor()
    {
        if (!FloorDetectorEnabled) return;
        _floorDetection = FloorDetectorLine.Detect2D(ObstacleLayers);
        if (!_floorDetection) MovementDirection = -MovementDirection;
    }

    private void DetectActor()
    {
        if (!ActorDetectorEnabled) return;
        if (SpottingActor)
        {
            MovementDirection = _actorDetection.collider.gameObject.transform.position.x > transform.position.x ? 1 : -1;
            return;
        }
        _actorDetection = ActorDetectorLine.Detect2D(ActorLayers);
        if (_actorDetection)
        {
            _actorSpottingStartTime = Time.time; 
            StartCoroutine(ThrowProjectile());
        }
    }

    private IEnumerator ThrowProjectile()
    {
        if (!Projectile) yield break;
        while (SpottingActor)
        {
            yield return new WaitForSeconds(1f);
            GameObject projectile = Instantiate(Projectile, ActorDetectorLine.Start, Quaternion.identity);
            Rigidbody2D projectileRigidBody2D = projectile.GetComponent<Rigidbody2D>();
            projectileRigidBody2D.gravityScale = 0f;
            projectileRigidBody2D.AddForce(MovementDirection * ProjectileForce * Vector2.right);
        }
    }

    #region Editor
    private void Reset()
    {
        WallDetectorLine = new DetectorLine(transform, from: new(0f, 0.2f, 0f), to: new(1f, 0.2f, 0f));
        FloorDetectorLine = new DetectorLine(transform, from: new(0f, 0.2f, 0f), to: new(2f, 0f, 0f));
        ActorDetectorLine = new DetectorLine(transform, from: new(0f, 1.3f, 0f), to: new(3f, 1.3f, 0f)); 
    }

    private void OnDrawGizmos()
    {
        if (WallDetectorEnabled) WallDetectorLine.DrawGizmo(_wallDetection ? Color.green : Color.red);
        if (FloorDetectorEnabled) FloorDetectorLine.DrawGizmo(_floorDetection ? Color.green : Color.red);
        if (ActorDetectorEnabled) ActorDetectorLine.DrawGizmo(SpottingActor ? Color.yellow : Color.red);
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
