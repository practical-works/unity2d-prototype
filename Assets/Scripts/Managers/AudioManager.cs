using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Main Child Audio Sources")]
    [SerializeField] private string _mainMusicAudioSourceName = "Music";
    [SerializeField] private string _mainSoundFXAudioSourceName = "SoundFX";
    private List<AudioSource> _musicAudioSources = new();
    private List<AudioSource> _soundFXAudioSources = new();
    [Header("Volume Settings")]
    [SerializeField] private bool _mute;
    [SerializeField] private bool _musicMute;
    [SerializeField] private bool _soundFXMute;
    [SerializeField][Range(0f, 1f)] private float _volume;
    [SerializeField][Range(0f, 1f)] private float _musicVolume;
    [SerializeField][Range(0f, 1f)] private float _soundFXVolume;

    public List<AudioSource> AudioSources => _musicAudioSources.Concat(_soundFXAudioSources).ToList();

    #region Mute Properties
    public bool Mute
    {
        get => GetMuteForAudioSources(AudioSources);
        set => SetMuteForAudioSources(AudioSources, value);
    }
    public bool MusicMute
    {
        get => GetMuteForAudioSources(_musicAudioSources);
        set => SetMuteForAudioSources(_musicAudioSources, value);
    }
    public bool SoundFXMute
    {
        get => GetMuteForAudioSources(_soundFXAudioSources);
        set => SetMuteForAudioSources(_soundFXAudioSources, value);
    }
    #endregion
    #region Volume Properties
    public float Volume
    {
        get => AudioListener.volume;
        set => AudioListener.volume = Mathf.Clamp(value, 0f, 1f);
    }
    public float MusicVolume
    {
        get => GetVolumeForAudioSources(_musicAudioSources);
        set => SetVolumeForAudioSources(_musicAudioSources, value);
    }
    public float SoundFXVolume
    {
        get => GetVolumeForAudioSources(_soundFXAudioSources);
        set => SetVolumeForAudioSources(_soundFXAudioSources, value);
    }
    #endregion
    #region Pause Properties
    public bool Pause
    {
        get => AudioListener.pause;
        set => AudioListener.pause = value;
    }
    public bool MusicPause
    {
        get => GetPauseForAudioSources(_musicAudioSources);
        set => SetPauseForAudioSources(_musicAudioSources, value);
    }
    public bool SoundFXPause
    {
        get => GetPauseForAudioSources(_soundFXAudioSources);
        set => SetPauseForAudioSources(_soundFXAudioSources, value);
    }
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
    }

    //private void OnEnable() => SceneManager.sceneLoaded += (_, _) => ClearNullAudioSources();

    //private void OnDisable() => SceneManager.sceneLoaded -= (_, _) => ClearNullAudioSources();

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
        if (_musicAudioSources.Count == 0)
        {
            Debug.LogWarning($"Could not find any child {nameof(AudioSource)} named \"{_mainMusicAudioSourceName}\".");
            _musicAudioSources.Add(CreateChildAudioSource(_mainMusicAudioSourceName));
            Debug.Log($"Created New child {nameof(AudioSource)} named \"{_mainMusicAudioSourceName}\".");
        }
        if (_soundFXAudioSources.Count == 0)
        {
            Debug.LogWarning($"Could not find any child {nameof(AudioSource)} named \"{_mainSoundFXAudioSourceName}\".");
            _soundFXAudioSources.Add(CreateChildAudioSource(_mainSoundFXAudioSourceName));
            Debug.Log($"Created New child {nameof(AudioSource)} named \"{_mainSoundFXAudioSourceName}\".");
        }
    }

    private AudioSource CreateChildAudioSource(string name)
    {
        GameObject audioSrcGameObj = new(name);
        audioSrcGameObj.transform.SetParent(transform);
        return audioSrcGameObj.AddComponent<AudioSource>();
    }

    private void ClearNullAudioSources()
    {
        _musicAudioSources = _musicAudioSources.Where(audioSrc => audioSrc != null).ToList();
        _soundFXAudioSources = _soundFXAudioSources.Where(audioSrc => audioSrc != null).ToList();
    }

    private void SetVolumeForAudioSources(List<AudioSource> audioSrcList, float volume)
    {
        foreach (AudioSource audioSrc in audioSrcList) audioSrc.volume = volume;
    }

    private float GetVolumeForAudioSources(List<AudioSource> audioSrcList)
    {
        return audioSrcList.Average(audioSrc => audioSrc.volume);
    }

    private void SetMuteForAudioSources(List<AudioSource> audioSrcList, bool mute)
    {
        foreach (AudioSource audioSrc in audioSrcList) audioSrc.mute = mute;
    }

    private bool GetMuteForAudioSources(List<AudioSource> audioSrcList)
    {
        return audioSrcList.TrueForAll(audioSrc => audioSrc.mute);
    }

    private void SetPauseForAudioSources(List<AudioSource> audioSrcList, bool pause)
    {
        foreach (AudioSource audioSrc in audioSrcList)
        {
            if (pause) audioSrc.Pause();
            else audioSrc.UnPause();
        }
    }

    private bool GetPauseForAudioSources(List<AudioSource> audioSrcList)
    {
        return audioSrcList.TrueForAll(audioSrc => !audioSrc.isPlaying && audioSrc.time != 0f);
    }

    private AudioSource CreateAudioSource(List<AudioSource> audioSrcList, AudioSource refAudioSrc, AudioClip clip, bool loop)
    {
        AudioSource audioSrc = refAudioSrc.gameObject.AddComponent<AudioSource>();
        audioSrcList.Add(audioSrc);
        audioSrc.volume = refAudioSrc.volume;
        audioSrc.pitch = refAudioSrc.pitch;
        audioSrc.clip = clip;
        audioSrc.loop = loop;
        return audioSrc;
    }

    private AudioSource Play(AudioSource audioSrc, AudioClip clip, bool oneshot = false, bool loop = false, float volume = -1f, float pitch = -1f)
    {
        float originalVolume = audioSrc.volume;
        float originalPitch = audioSrc.pitch;
        if (volume >= 0f) audioSrc.volume = volume;
        if (pitch >= 0f) audioSrc.pitch = pitch;
        if (oneshot && loop && audioSrc.clip)
        {
            AudioSource newAudioSrc = CreateAudioSource(_soundFXAudioSources, audioSrc, clip, loop);
            newAudioSrc.Play();
            OnValidate();
            newAudioSrc.volume = originalVolume;
            newAudioSrc.pitch = originalPitch;
            return newAudioSrc;
        }
        else if (oneshot && !loop) audioSrc.PlayOneShot(clip);
        else
        {
            audioSrc.clip = clip;
            audioSrc.loop = loop;
            audioSrc.Play();
        }
        audioSrc.volume = originalVolume;
        audioSrc.pitch = originalPitch;
        return audioSrc;
    }
    #endregion

    public AudioSource PlayMusic(AudioClip musicClip, bool loop = false, float volume = -1f, float pitch = -1f)
        => Play(_musicAudioSources[0], musicClip, oneshot: false, loop, volume, pitch);

    public AudioSource PlaySoundFX(AudioClip soundFXClip, bool loop = false, float volume = -1f, float pitch = -1f)
        => Play(_soundFXAudioSources[0], soundFXClip, oneshot: true, loop, volume, pitch);

    public AudioSource PlayMusicLocally(AudioSource localMusicSrc, AudioClip musicClip, bool loop = false, float volume = -1f, float pitch = -1f)
    {
        if (!_musicAudioSources.Contains(localMusicSrc)) _musicAudioSources.Add(localMusicSrc);
        return Play(localMusicSrc, musicClip, oneshot: false, loop, volume, pitch);
    }

    public AudioSource PlaySoundFXLocally(AudioSource localSoundFXSrc, AudioClip soundFXClip, bool loop = false, float volume = -1f, float pitch = -1f)
    {
        if (!_soundFXAudioSources.Contains(localSoundFXSrc)) _soundFXAudioSources.Add(localSoundFXSrc);
        return Play(localSoundFXSrc, soundFXClip, oneshot: true, loop, volume, pitch);
    }

    public AudioSource ResumeClip(AudioClip clip)
    {
        AudioSource audioSrc = AudioSources.Find(audioSrc => audioSrc.clip == clip);
        audioSrc.UnPause();
        return audioSrc;
    }

    public AudioSource PauseClip(AudioClip clip)
    {
        AudioSource audioSrc = AudioSources.Find(audioSrc => audioSrc.clip == clip);
        audioSrc.Pause();
        return audioSrc;
    }

    public AudioSource StopClip(AudioClip clip)
    {
        AudioSource audioSrc = AudioSources.Find(audioSrc => audioSrc.clip == clip);
        audioSrc.Stop();
        return audioSrc;
    }

    #region Inspector Methods
    private void Reset()
    {
        RetrieveChildAudioSources();
        _mute = _musicMute = _soundFXMute = false;
        _volume = _musicVolume = _soundFXVolume = 1f;
    }

    internal override void OnValidate()
    {
        Mute = _mute;
        MusicMute = _musicMute | _mute;
        SoundFXMute = _soundFXMute | _mute;
        Volume = _volume;
        MusicVolume = _musicVolume;
        SoundFXVolume = _soundFXVolume;
    }
    #endregion
}
