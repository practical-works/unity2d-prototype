using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public AudioClip Music;

    private void Start()
    {
        if (Music) AudioManager.Instance.PlayMusic(Music);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
