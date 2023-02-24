using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public AudioClip Music;
    public AudioClip AmbientSound;
    public AudioClip Voice;

    private void Start()
    {
        if (Music) AudioManager.Instance.PlayMusic(Music, loop: true); 
        if (AmbientSound) AudioManager.Instance.PlaySoundFX(AmbientSound, loop: true);
        if (Voice) AudioManager.Instance.PlaySoundFX(Voice, loop: true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        if (Input.GetKeyDown(KeyCode.Keypad7))
            AudioManager.Instance.Mute = !AudioManager.Instance.Mute;
        if (Input.GetKeyDown(KeyCode.Keypad4))
            AudioManager.Instance.MusicMute = !AudioManager.Instance.MusicMute;
        if (Input.GetKeyDown(KeyCode.Keypad1))
            AudioManager.Instance.SoundFXMute = !AudioManager.Instance.SoundFXMute;

        if (Input.GetKey(KeyCode.Keypad8))
            AudioManager.Instance.Volume -= 0.1f * Time.deltaTime;
        if (Input.GetKey(KeyCode.Keypad5))
            AudioManager.Instance.MusicVolume -= 0.1f * Time.deltaTime;
        if (Input.GetKey(KeyCode.Keypad2))
            AudioManager.Instance.SoundFXVolume -= 0.1f * Time.deltaTime;

        if (Input.GetKey(KeyCode.Keypad9))
            AudioManager.Instance.Volume += 0.1f * Time.deltaTime;
        if (Input.GetKey(KeyCode.Keypad6))
            AudioManager.Instance.MusicVolume += 0.1f * Time.deltaTime;
        if (Input.GetKey(KeyCode.Keypad3))
            AudioManager.Instance.SoundFXVolume += 0.1f * Time.deltaTime;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(x: 10f, y: 10f, width: 200f, height: 20f),
            $"All: {AudioManager.Instance.VolumePercent} %{(AudioManager.Instance.Mute ? " (Muted)" : "")}");
        GUI.Label(new Rect(x: 10f, y: 30f, width: 200f, height: 20f), 
            $"Music: {AudioManager.Instance.MusicVolumePercent} %{(AudioManager.Instance.MusicMute ? " (Muted)" : "")}");
        GUI.Label(new Rect(x: 10f, y: 50f, width: 200f, height: 20f), 
            $"SoundFX: {AudioManager.Instance.SoundFXVolumePercent} %{(AudioManager.Instance.SoundFXMute ? " (Muted)" : "")}");
    }
}
