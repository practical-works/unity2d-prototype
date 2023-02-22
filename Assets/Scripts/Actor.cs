using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Exploder2D))]
public class Actor : MonoBehaviour
{
    [Header("Stats")]
    [Range(0, 100)] public int Health = 3;
    [Header("Battle")]
    public string[] DamageSourcesTags = new[] { "Damage" };
    [Min(0)] public float DamageInvincibilitySeconds = 0.5f;
    [Header("Animator")]
    public string DamagedAnimStateName = "Actor_Damaged";
    [Header("Audio")]
    public AudioClip HitSoundFX;

    private Animator _animator;
    private Exploder2D _exploder;
    private bool _invincible;


    private void Awake()
    {   
        _animator = GetComponent<Animator>();
        _exploder = GetComponent<Exploder2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (string damageSourceTag in DamageSourcesTags)
            if (collision.gameObject.CompareTag(damageSourceTag))
                if (!_invincible) StartCoroutine(Damage(collision.gameObject));
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        foreach (string damageSourceTag in DamageSourcesTags)
            if (collider.gameObject.CompareTag(damageSourceTag))
                if (!_invincible) StartCoroutine(Damage(collider.gameObject));
    }

    private IEnumerator Damage(GameObject _)
    {
        Health--;
        AudioManager.Instance.PlaySoundEffect(HitSoundFX);
        _animator.Play(DamagedAnimStateName);
        if (Health <= 0)
        {
            Health = 0;
            _exploder.Explode();
            yield return null;
        }
        _invincible = true;
        yield return new WaitForSeconds(DamageInvincibilitySeconds);
        _invincible = false;
    }
}
