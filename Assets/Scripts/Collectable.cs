using UnityEngine;

public class Collectable : MonoBehaviour
{
    public AudioClip CollectingSoundFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySoundFX(CollectingSoundFX);
            Destroy(gameObject);
        }
    }
}
