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
    public AudioPlayer BackgroundMusic;
    public AudioPlayer AmbientSoundFX;
    public AudioPlayer VoiceSoundFX;
    public AudioPlayer CursorSoundFX;
    public AudioPlayer ConfirmSoundFX;

    private bool _pendingStartGame;

    private void Awake()
    {
        BackgroundMusic.Play();
        AmbientSoundFX.Play();
        VoiceSoundFX.Play();
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
        StartGameText.enabled = false;
        BackgroundMusic.Stop();
        AmbientSoundFX.Stop();
        VoiceSoundFX.Stop();
        ConfirmSoundFX.Play();
        yield return new WaitWhile(() => ConfirmSoundFX.IsPlaying);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _pendingStartGame = false;
    }
}
