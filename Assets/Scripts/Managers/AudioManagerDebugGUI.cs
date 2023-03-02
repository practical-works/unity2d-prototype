using UnityEngine;

public class AudioManagerDebugGUI : Singleton<AudioManagerDebugGUI>
{
    public bool ShowGUI = true;
    public bool EnableControl = true;
    [Header("GUI")]
    public Color TextColor = Color.white;
    public Rect PositionPixels = new(x: 10f, y: 10f, width: 200f, height: 20f);
    [Header("Control Keys")]
    public KeyCode AllMuteKey = KeyCode.Keypad7;
    public KeyCode MusicMuteKey = KeyCode.Keypad4;
    public KeyCode SoundFXMuteKey = KeyCode.Keypad1;
    public KeyCode AllVolumeDownKey = KeyCode.Keypad8;
    public KeyCode MusicVolumeDownKey = KeyCode.Keypad5;
    public KeyCode SoundFXVolumeDownKey = KeyCode.Keypad2;
    public KeyCode AllVolumeUpKey = KeyCode.Keypad9;
    public KeyCode MusicVolumeUpKey = KeyCode.Keypad6;
    public KeyCode SoundFXVolumeUpKey = KeyCode.Keypad3;

    public void Update()
    {
        if (!EnableControl) return;
        // Mute Audio
        if (Input.GetKeyDown(AllMuteKey))
            AudioManager.Instance.Mute = !AudioManager.Instance.Mute;
        if (Input.GetKeyDown(MusicMuteKey))
            AudioManager.Instance.MusicMute = !AudioManager.Instance.MusicMute;
        if (Input.GetKeyDown(SoundFXMuteKey))
            AudioManager.Instance.SoundFXMute = !AudioManager.Instance.SoundFXMute;
        // Increment Audio Volume
        if (Input.GetKey(AllVolumeDownKey))
            AudioManager.Instance.Volume -= 0.1f * Time.deltaTime;
        if (Input.GetKey(MusicVolumeDownKey))
            AudioManager.Instance.MusicVolume -= 0.1f * Time.deltaTime;
        if (Input.GetKey(SoundFXVolumeDownKey))
            AudioManager.Instance.SoundFXVolume -= 0.1f * Time.deltaTime;
        // Decrement Audio Volume
        if (Input.GetKey(AllVolumeUpKey))
            AudioManager.Instance.Volume += 0.1f * Time.deltaTime;
        if (Input.GetKey(MusicVolumeUpKey))
            AudioManager.Instance.MusicVolume += 0.1f * Time.deltaTime;
        if (Input.GetKey(SoundFXVolumeUpKey))
            AudioManager.Instance.SoundFXVolume += 0.1f * Time.deltaTime;
    }

    public void OnGUI()
    {
        if (!ShowGUI) return;
        GUI.color = TextColor;
        GUI.Label(PositionPixels, "[Audio Debug Controller]");
        GUI.Label(new Rect(PositionPixels.x, PositionPixels.y + 20f, PositionPixels.width, PositionPixels.height),
            $"All: {AudioManager.Instance.VolumePercent} %{(AudioManager.Instance.Mute ? " (Muted)" : "")}");
        GUI.Label(new Rect(PositionPixels.x, PositionPixels.y + 40f, PositionPixels.width, PositionPixels.height),
            $"Music: {AudioManager.Instance.MusicVolumePercent} %{(AudioManager.Instance.MusicMute ? " (Muted)" : "")}");
        GUI.Label(new Rect(PositionPixels.x, PositionPixels.y + 60f, PositionPixels.width, PositionPixels.height),
            $"SoundFX: {AudioManager.Instance.SoundFXVolumePercent} %{(AudioManager.Instance.SoundFXMute ? " (Muted)" : "")}");
    }

    private void Reset()
    {
        hideFlags = HideFlags.DontSaveInBuild;
    }
}
