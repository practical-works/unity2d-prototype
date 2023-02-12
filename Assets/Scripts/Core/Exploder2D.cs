using System.Collections;
using UnityEngine;

public class Exploder2D : MonoBehaviour
{
    public GameObject Fragment;
    public int ShrapnelsCount = 5;
    [Header("Physics")]
    public Vector2 ForceMin = new(-100, 100);
    public Vector2 ForceMax = new(100, 300);
    [Header("Cleanup")]
    public bool DestroyGameObject = true;
    public bool DestroyShrapnels = true;
    public float ShrapnelLifeSeconds = 2f;
    public bool FadeOutEffect = true;
    public float FadeOutSeconds = 3f;

    private SpriteRenderer _spriteRenderer;

    [ContextMenu("Explode")]
    public void Explode()
    {
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
        GameObject shrapnel = Instantiate(Fragment, position, Quaternion.identity);
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
        shrapnelRigidBody2D.AddForce(Vector2.right * Random.Range(ForceMin.x, ForceMax.x));
        shrapnelRigidBody2D.AddForce(Vector2.up * Random.Range(ForceMin.y, ForceMax.y));
    }

    private IEnumerator DisposeShrapnel(SpriteRenderer shrapnelSpriteRenderer)
    {
        yield return new WaitForSeconds(ShrapnelLifeSeconds);
        if (FadeOutEffect) yield return FadeOutShrapnel(shrapnelSpriteRenderer);
        Destroy(shrapnelSpriteRenderer.gameObject);
    }

    private IEnumerator FadeOutShrapnel(SpriteRenderer shrapnelSpriteRenderer)
    {
        Color initialColor = shrapnelSpriteRenderer.color;
        Color finalColor = new(initialColor.r, initialColor.g, initialColor.b, 0f);
        float i = 0f;
        while (shrapnelSpriteRenderer.color.a > 0f)
        {
            yield return new WaitForEndOfFrame();
            i += Time.deltaTime / FadeOutSeconds;
            shrapnelSpriteRenderer.color = Color.Lerp(initialColor, finalColor, i);
        }
    }
}
