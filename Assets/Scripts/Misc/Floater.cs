using UnityEngine;

public class Floater : MonoBehaviour
{
    [field: SerializeField] public Vector3 DistanceScale { get; private set; } = new(0f, 3f, 0f);
    [field: SerializeField] public Vector3 MotionSpeed { get; private set; } = new(0f, 6f, 0f);

    private Vector3 _initialPosition;

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3()
        {
            x = GetNextCoordinate(_initialPosition.x, DistanceScale.x, MotionSpeed.x),
            y = GetNextCoordinate(_initialPosition.y, DistanceScale.y, MotionSpeed.y),
            z = GetNextCoordinate(_initialPosition.z, DistanceScale.z, MotionSpeed.z)
        };
    }

    private float GetNextCoordinate(float initialCoordinate, float distanceScale, float motionSpeed) 
        => initialCoordinate + (Mathf.Lerp(0f, distanceScale, Time.time) * Mathf.Cos(Time.time / 2f * motionSpeed) / 4f);
}
