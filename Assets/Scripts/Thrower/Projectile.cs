using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Cleanup")]
    public bool DestroyGameObject = true;
    public float LifeSeconds = 2f;
    [Header("Cleanup / Fade Out")]
    public bool FadeOut = true;
    public float FadeOutSeconds = 3f;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(DisposeProjectile(_spriteRenderer));
    }

    private IEnumerator DisposeProjectile(SpriteRenderer spriteRenderer)
    {
        yield return new WaitForSeconds(LifeSeconds);
        if (FadeOut) SpriteFader.FadeOut(spriteRenderer, FadeOutSeconds, () => Destroy(gameObject));
        else Destroy(gameObject);
    }
}
