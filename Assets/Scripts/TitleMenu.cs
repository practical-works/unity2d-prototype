using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI StartGameText;
    [Header("Audio")]
    public AudioClip BackgroundMusic;
    public AudioClip AmbientSoundFX;
    public AudioClip VoiceSoundFX;
    public AudioClip CursorSoundFX;
    public AudioClip ConfirmSoundFX;
    public AudioPlayer BackgroundMusicAudio;

    private bool _pendingStartGame;

    private void Awake()
    {
        if (BackgroundMusic) AudioManager.Instance.PlayMusic(BackgroundMusic, loop: true);
        if (AmbientSoundFX) AudioManager.Instance.PlaySoundFX(AmbientSoundFX, loop: true);
        if (VoiceSoundFX) AudioManager.Instance.PlaySoundFX(VoiceSoundFX, loop: true);
    }

    void Update()
    {
        if (Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(3))
            StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        if (_pendingStartGame) yield break;
        _pendingStartGame = true;
        yield return new WaitForSeconds(PlayConfirmSoundFX());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _pendingStartGame = false;
    }

    private float PlayConfirmSoundFX()
    {
        if (!ConfirmSoundFX) return 1f;
        float pitch = 0.3f;
        StartGameText.enabled = false;
        AudioManager.Instance.StopClip(BackgroundMusic);
        AudioManager.Instance.PlaySoundFX(ConfirmSoundFX, false, -1, pitch);
        return ConfirmSoundFX.length / pitch;
    }
}
