using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Platformer2D), typeof(DetectorLine2D), typeof(DetectorLine2D))]
public class Platformer2DAutomator : MonoBehaviour
{
    [Header("Movement")]
    [Range(-1f, 1f)] public int MovementDirection = 1;
    [Header("Turn Round")]
    public bool TurnRoundAtWall = true;
    public bool TurnRoundAtNoFloor = true;
    [Header("Follow")]
    public ActorFollowingMode FollowActor = ActorFollowingMode.OnDetection;
    public float ActorFollowSeconds = 10f;
    public float ActorFollowStartDistance = 0.1f;
    public Transform ActorTransform;
    [Header("Detectors")]
    public string WallDetectorID = "Wall";
    public string FloorDetectorID = "Floor";
    public string ActorDetectorID = "Actor";

    public enum ActorFollowingMode { Never, Always, OnDetection }

    private Platformer2D _platformer2D;
    private DetectorLine2D _wallDetectorLine2D;
    private DetectorLine2D _floorDetectorLine2D;
    private DetectorLine2D _actorDetectorLine2D;
    private RaycastHit2D _actorDetection;
    private float _actorSpottingStartTime;

    public bool SpottingActor => _actorDetection && Time.time < _actorSpottingStartTime;

    private void Awake()
    {
        _platformer2D = GetComponent<Platformer2D>();
        if (FollowActor == ActorFollowingMode.Always && !ActorTransform)
            Debug.LogWarning($"{nameof(FollowActor)}:{nameof(ActorFollowingMode.Always)} requires target {nameof(ActorTransform)}");
        RetrieveDetectors();
    }

    private void FixedUpdate()
    {
        if (!_platformer2D.IsFalling) _platformer2D.Move(MovementDirection);
        if (TurnRoundAtWall && _wallDetectorLine2D.ObstacleDetection) InvertMovementDirection();
        if (TurnRoundAtNoFloor && !_floorDetectorLine2D.ObstacleDetection) InvertMovementDirection();
        if (FollowActor == ActorFollowingMode.Always && ActorTransform) MoveTowardsActor();
        if (FollowActor == ActorFollowingMode.OnDetection)
        {
            if (SpottingActor) MoveTowardsActor();
            else if (_actorDetection = _actorDetectorLine2D.ObstacleDetection)
            {
                ActorTransform = _actorDetection.collider.gameObject.transform;
                _actorSpottingStartTime = Time.time + ActorFollowSeconds;
                MoveTowardsActor();
            }
        }
    }

    private void InvertMovementDirection() => MovementDirection = -MovementDirection;

    private void MoveTowardsActor()
    {
        int direction = 0;
        if (ActorTransform.position.x > transform.position.x + ActorFollowStartDistance) direction = 1;
        if (ActorTransform.position.x < transform.position.x - ActorFollowStartDistance) direction = -1;
        if (ActorTransform.position.x == transform.position.x) direction = 0;
        MovementDirection = direction;
    }

    private void RetrieveDetectors()
    {
        DetectorLine2D[] detectors = GetComponents<DetectorLine2D>();
        foreach (DetectorLine2D detector in detectors)
        {
            if (detector.ID.ToLower() == WallDetectorID.ToLower()) _wallDetectorLine2D = detector;
            if (detector.ID.ToLower() == FloorDetectorID.ToLower()) _floorDetectorLine2D = detector;
            if (detector.ID.ToLower() == ActorDetectorID.ToLower()) _actorDetectorLine2D = detector;
        }
        WarnForMissingDetectors();
    }

    private void WarnForMissingDetectors()
    {
        if (_wallDetectorLine2D && _floorDetectorLine2D && _actorDetectorLine2D) return;
        void WarnForMissingDetector(DetectorLine2D detector, string detectorID)
        {
            if (detector) return;
            Debug.LogWarning($"Could not find {nameof(DetectorLine2D)} of ID\"{detectorID}\" in {gameObject.name}");
        }
        WarnForMissingDetector(_wallDetectorLine2D, WallDetectorID);
        WarnForMissingDetector(_floorDetectorLine2D, FloorDetectorID);
        WarnForMissingDetector(_actorDetectorLine2D, ActorDetectorID);
    }

    private void OnDrawGizmos()
    {
        if (SpottingActor)
        {
            Handles.Label(new Vector3(transform.position.x, transform.position.y + 2f, 0f), 
                $"{ActorTransform.name}! {Mathf.Round(_actorSpottingStartTime - Time.time)}s...");
        }
    }
}


