using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject Projectile;
    public float ProjectileForce = 10f;

    [ContextMenu("Throw Projectile")]
    public void ThrowProjectile()
    {

    }
}
