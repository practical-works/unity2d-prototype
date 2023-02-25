using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public AudioClip Music;
    public AudioClip AmbientSound;
    public AudioClip Voice;

    public bool EnterKeyDown => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);

    private void Start()
    {
        if (Music) AudioManager.Instance.PlayMusic(Music, loop: true);
        if (AmbientSound) AudioManager.Instance.PlaySoundFX(AmbientSound, loop: true);
        if (Voice) AudioManager.Instance.PlaySoundFX(Voice, loop: true);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (EnterKeyDown) LoadNextScene();
        }
        else
        {
            if (EnterKeyDown) ReloadScene();
        }
    }

    private void LoadNextScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    private void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
