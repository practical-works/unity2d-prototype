using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorLine2D : MonoBehaviour
{
    public string ID = "";
    public LayerMask ObstacleLayers = 1 << 3;
    public Vector3 LocalStart;
    public Vector3 LocalEnd;

    private bool _obstacleDetected;

    public Vector3 Start => transform.TransformPoint(LocalStart);
    public Vector3 End => transform.TransformPoint(LocalEnd);
    public RaycastHit2D ObstacleDetection => Physics2D.Linecast(Start, End, ObstacleLayers);

    private void Update() => _obstacleDetected = !!ObstacleDetection;

    private void Reset()
    {
        LocalStart = new Vector3(0f, 0f, 0f);
        LocalEnd = new Vector3(1f, 1f, 0f);
    }

    private void OnDrawGizmos()
    {
        if (!isActiveAndEnabled) return;
        Gizmos.color = _obstacleDetected ? Color.green : Color.red;
        Gizmos.DrawLine(Start, End);
    }
}
