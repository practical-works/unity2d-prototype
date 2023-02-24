using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Main Child Audio Sources")]
    [SerializeField] private string _mainMusicAudioSourceName = "Music";
    [SerializeField] private string _mainSoundFXAudioSourceName = "SoundFX";
    private readonly List<AudioSource> _musicAudioSources = new();
    private readonly List<AudioSource> _soundFXAudioSources = new();
    private float _audioListenerVolumeBeforeMute;

    #region Volume Properties
    public float Volume { get => AudioListener.volume; set => AudioListener.volume = Mathf.Clamp(value, 0f, 1f); }
    public float MusicVolume { get => _musicAudioSources[0].volume; set => SetVolumeForAudioSources(_musicAudioSources, value); }
    public float SoundFXVolume { get => _soundFXAudioSources[0].volume; set => SetVolumeForAudioSources(_soundFXAudioSources, value); }
    #endregion
    #region Mute Properties
    public bool Mute { get => AudioListener.volume == 0f; set { MuteAudioListener(value); } }
    public bool MusicMute { get => _musicAudioSources[0].mute; set => SetMuteForAudioSources(_musicAudioSources, value); }
    public bool SoundFXMute { get => _soundFXAudioSources[0].mute; set => SetMuteForAudioSources(_soundFXAudioSources, value); }
    #endregion
    #region Pause Properties
    public bool Pause { get => AudioListener.pause; set => AudioListener.pause = value; }
    public bool MusicPause { get => AudioSourceIsPaused(_musicAudioSources[0]); set => SetPauseForAudioSources(_musicAudioSources, value); }
    public bool SoundFXPause { get => AudioSourceIsPaused(_soundFXAudioSources[0]); set => SetPauseForAudioSources(_soundFXAudioSources, value); }
    #endregion
    #region Volume Percentage Properties
    public float VolumePercent { get => Mathf.Round(100f * AudioListener.volume); }
    public float MusicVolumePercent { get => Mathf.Round(100f * _musicAudioSources[0].volume); }
    public float SoundFXVolumePercent { get => Mathf.Round(100f * _soundFXAudioSources[0].volume); }
    #endregion

    internal override void Awake()
    {
        base.Awake();
        RetrieveChildAudioSources();
        CreateChildAudioSources();
    }

    #region Helper Methods
    private void RetrieveChildAudioSources()
    {
        AudioSource[] childAudioSrcs = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audioSrc in childAudioSrcs)
        {
            if (audioSrc.name.Contains(_mainMusicAudioSourceName, StringComparison.InvariantCultureIgnoreCase))
                _musicAudioSources.Add(audioSrc);
            if (audioSrc.name.Contains(_mainSoundFXAudioSourceName, StringComparison.InvariantCultureIgnoreCase))
                _soundFXAudioSources.Add(audioSrc);
        }
    }

    private void CreateChildAudioSources()
    {
        if (_musicAudioSources.Count == 0)
        {
            _musicAudioSources.Add(CreateAudioSource(_mainMusicAudioSourceName));
            Debug.LogWarning($"Could not find any child {nameof(AudioSource)} named \"{_mainMusicAudioSourceName}\".");
            Debug.Log($"Created New child {nameof(AudioSource)} named \"{_mainMusicAudioSourceName}\".");
        }
        if (_soundFXAudioSources.Count == 0 || !_soundFXAudioSources[0])
        {
            _soundFXAudioSources.Add(CreateAudioSource(_mainSoundFXAudioSourceName));
            Debug.LogWarning($"Could not find any child {nameof(AudioSource)} named \"{_mainSoundFXAudioSourceName}\".");
            Debug.Log($"Created New child {nameof(AudioSource)} named \"{_mainSoundFXAudioSourceName}\".");
        }
    }

    private AudioSource CreateAudioSource(string name)
    {
        GameObject audioSrcGameObj = new(name);
        audioSrcGameObj.transform.SetParent(transform);
        return audioSrcGameObj.AddComponent<AudioSource>();
    }

    private void SetVolumeForAudioSources(List<AudioSource> audioSrcList, float volume)
    {
        foreach (AudioSource audioSrc in audioSrcList) audioSrc.volume = volume;
    }

    private void SetMuteForAudioSources(List<AudioSource> audioSrcList, bool mute)
    {
        foreach (AudioSource audioSrc in audioSrcList) audioSrc.mute = mute;
    }

    private void SetPauseForAudioSources(List<AudioSource> audioSrcList, bool pause)
    {
        foreach (AudioSource audioSrc in audioSrcList)
        {
            if (pause) audioSrc.Pause();
            else audioSrc.UnPause();
        }
    }

    private bool AudioSourceIsPaused(AudioSource audioSrc)
    {
        return !audioSrc.isPlaying && audioSrc.time != 0f;
    }

    private void MuteAudioListener(bool mute)
    {
        if (AudioListener.volume > 0f) _audioListenerVolumeBeforeMute = AudioListener.volume;
        AudioListener.volume = mute ? 0f : _audioListenerVolumeBeforeMute;
    }

    private void Play(AudioSource audioSrc, AudioClip clip, bool oneshot = false, bool loop = false, float volume = -1f, float pitch = -1f)
    {
        if (volume >= 0f) audioSrc.volume = volume;
        if (pitch >= 0f) audioSrc.pitch = pitch;
        if (oneshot && loop && audioSrc.clip)
        {
            AudioSource newAudioSrc = audioSrc.gameObject.AddComponent<AudioSource>();
            _soundFXAudioSources.Add(newAudioSrc);
            newAudioSrc.volume = audioSrc.volume;
            newAudioSrc.pitch = audioSrc.pitch;
            newAudioSrc.clip = clip;
            newAudioSrc.loop = loop;
            newAudioSrc.Play();
        }
        else if (oneshot && !loop) audioSrc.PlayOneShot(clip);
        else
        {
            audioSrc.clip = clip;
            audioSrc.loop = loop;
            audioSrc.Play();
        }
    }
    #endregion

    public void PlayMusic(AudioClip musicClip, bool loop = false, float volume = -1f, float pitch = -1f)
        => Play(_musicAudioSources[0], musicClip, oneshot: false, loop, volume, pitch);

    public void PlaySoundFX(AudioClip soundFXClip, bool loop = false, float volume = -1f, float pitch = -1f)
        => Play(_soundFXAudioSources[0], soundFXClip, oneshot: true, loop, volume, pitch);

    public void PlayMusicLocally(AudioSource localMusicSrc, AudioClip musicClip, bool loop = false, float volume = -1f, float pitch = -1f)
    {
        if (!_musicAudioSources.Contains(localMusicSrc)) _musicAudioSources.Add(localMusicSrc);
        Play(localMusicSrc, musicClip, oneshot: false, loop, volume, pitch);
    }

    public void PlaySoundFXLocally(AudioSource localSoundFXSrc, AudioClip soundFXClip, bool loop = false, float volume = -1f, float pitch = -1f)
    {
        if (!_soundFXAudioSources.Contains(localSoundFXSrc)) _soundFXAudioSources.Add(localSoundFXSrc);
        Play(localSoundFXSrc, soundFXClip, oneshot: true, loop, volume, pitch);
    }
}
