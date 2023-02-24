using System.Collections;
using UnityEngine;

public class Exploder2D : MonoBehaviour
{
    public bool DestroyGameObject = true;
    [Header("Shrapnels")]
    public GameObject Shrapnel;
    public int ShrapnelsCount = 5;
    [Header("Shrapnels / Cleanup")]
    public bool DestroyShrapnels = true;
    public float ShrapnelLifeSeconds = 2f;
    [Header("Shrapnels / Cleanup / Fade Out")]
    public bool ShrapnelFadeOut = true;
    public float ShrapnelFadeOutSeconds = 3f;
    [Header("Physics")]
    public Vector2 ExplosionForceMin = new(-100, 100);
    public Vector2 ExplosionForceMax = new(100, 300);
    [Header("Audio")]
    public AudioClip ExplosionSoundFx;

    private SpriteRenderer _spriteRenderer;

    [ContextMenu("Explode")]
    public void Explode()
    {
        if (ExplosionSoundFx) AudioManager.Instance.PlaySoundFX(ExplosionSoundFx);
        if (DestroyGameObject) Destroy(gameObject);
        for (int i = 0; i < ShrapnelsCount; i++)
        {
            var shrapnel = CreateShrapnel();
            AdjustShrapnel(shrapnel.SpriteRenderer);
            PushAwayShrapnel(shrapnel.Rigidbody2D);
            if (DestroyShrapnels) GameManager.Instance.StartCoroutine(DisposeShrapnel(shrapnel.SpriteRenderer));
        }
    }

    private (GameObject GameObject, SpriteRenderer SpriteRenderer, Rigidbody2D Rigidbody2D) CreateShrapnel()
    {
        Vector3 position = (_spriteRenderer || TryGetComponent(out _spriteRenderer)) ? 
            _spriteRenderer.bounds.center : transform.position;
        GameObject shrapnel = Instantiate(Shrapnel, position, Quaternion.identity);
        SpriteRenderer shrapnelSpriteRenderer = shrapnel.GetComponent<SpriteRenderer>();
        Rigidbody2D shrapnelRigidBody2D = shrapnel.GetComponent<Rigidbody2D>();
        return (shrapnel, shrapnelSpriteRenderer, shrapnelRigidBody2D);
    }

    private void AdjustShrapnel(SpriteRenderer shrapnelSpriteRenderer)
    {
        shrapnelSpriteRenderer.color = _spriteRenderer.color;
        shrapnelSpriteRenderer.gameObject.layer = gameObject.layer;
    }

    private void PushAwayShrapnel(Rigidbody2D shrapnelRigidBody2D)
    {
        shrapnelRigidBody2D.AddForce(Vector2.right * Random.Range(ExplosionForceMin.x, ExplosionForceMax.x));
        shrapnelRigidBody2D.AddForce(Vector2.up * Random.Range(ExplosionForceMin.y, ExplosionForceMax.y));
    }

    private IEnumerator DisposeShrapnel(SpriteRenderer shrapnelSpriteRenderer)
    {
        yield return new WaitForSeconds(ShrapnelLifeSeconds);
        if (ShrapnelFadeOut) SpriteFader.FadeOut(shrapnelSpriteRenderer, ShrapnelFadeOutSeconds, () => Destroy(shrapnelSpriteRenderer.gameObject));
        else Destroy(shrapnelSpriteRenderer.gameObject);
    }
}
