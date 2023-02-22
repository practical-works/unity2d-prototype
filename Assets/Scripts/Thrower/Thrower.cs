using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject Projectile;
    public Vector3 ProjectileLocalScale = new(0.5f, 0.5f, 1f);
    [Header("Throwing")]
    public Vector3 ThrowingLocalOrigin = new(0f, 0f, 0f);
    public Vector3 ThrowingForce = new(1f, 0f, 0f);
    public bool UsePhysics2D = true;
    [Header("Auto-Throw")]
    public bool AutoThrow;
    public float AutoThrowDelaySeconds = 1f;
    [Header("Audio")]
    public AudioClip ThrowingSoundFX;

    private AudioSource _audioSource;
    private bool _autoThrowing;

    public Vector3 ThrowingOrigin => transform.TransformPoint(ThrowingLocalOrigin);
    public Vector3 ScaledThrowingForce => Vector3.Scale(ThrowingForce, transform.localScale.normalized);

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    private void Update()
    {
        if (!_autoThrowing && AutoThrow) StartCoroutine(AutoThrowProjectile());
    }

    private IEnumerator MoveProjectile(GameObject projectile)
    {
        while (true)
        {
            projectile.transform.position += 0.01f * ScaledThrowingForce;
            yield return new WaitForFixedUpdate();
        }
    }

    public void MoveProjectileWithPhysics2D(GameObject projectile)
    {
        if (!projectile.TryGetComponent(out Rigidbody2D rigidBody2D))
        {
            Debug.LogError($"Could not find {nameof(Rigidbody2D)} component on projectile \"{projectile.name}\"");
            return;
        }
        rigidBody2D.AddForce(100f * ScaledThrowingForce);
    }

    public IEnumerator AutoThrowProjectile()
    {
        _autoThrowing = true;
        while (AutoThrow)
        {
            yield return new WaitForSeconds(AutoThrowDelaySeconds);
            ThrowProjectile();
        }
        _autoThrowing = false;
    }

    [ContextMenu("Throw Projectile")]
    public void ThrowProjectile()
    {
        GameObject projectile = Instantiate(Projectile, ThrowingOrigin, Projectile.transform.rotation);
        projectile.transform.localScale = ProjectileLocalScale;
        if (UsePhysics2D) MoveProjectileWithPhysics2D(projectile);
        else StartCoroutine(MoveProjectile(projectile));
        if (ThrowingSoundFX)
        {
            if (_audioSource) AudioManager.Instance.PlaySoundEffectLocally(_audioSource, ThrowingSoundFX);
            else AudioManager.Instance.PlaySoundEffect(ThrowingSoundFX);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(ThrowingOrigin, 0.1f * Vector3.one);
        Gizmos.DrawRay(ThrowingOrigin, ScaledThrowingForce);
    }
}
