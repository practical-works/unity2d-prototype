using UnityEngine;

[RequireComponent(typeof(Thrower))]
public class ThrowerController : MonoBehaviour
{
    [Header("Controls")]
    public string ThrowButtonName = "Fire3";

    private Thrower _thrower;

    private void Awake() => _thrower = GetComponent<Thrower>();

    private void Update()
    {
        if (Input.GetButtonDown(ThrowButtonName)) _thrower.ThrowProjectile();
    }
}
