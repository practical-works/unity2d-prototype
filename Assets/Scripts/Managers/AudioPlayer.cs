using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioPlayer
{
    private static AudioPlayerContainer s_audioPlayerContainer;
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField, EnumPaging] private PlaybackMode _playbackMode;
    [SerializeField, ToggleLeft] private bool _interruptOnPlay;
    [SerializeField, FoldoutGroup(nameof(Volume)), ToggleLeft] private bool _randomizeVolume;
    [SerializeField, FoldoutGroup(nameof(Pitch)), ToggleLeft] private bool _randomizePitch;
    [SerializeField, FoldoutGroup(nameof(Volume)), MinMaxSlider(0f, 1f), ShowIf(nameof(_randomizeVolume))]
    private Vector2 _volumeRange = new(0.5f, 1f);
    [SerializeField, FoldoutGroup(nameof(Pitch)), MinMaxSlider(-3f, 3f), ShowIf(nameof(_randomizePitch))]
    private Vector2 _pitchRange = new(-1f, 1f);
    private AudioSource _audioSource;
    private IEnumerator<AudioClip> _cyclicPlayer;

    public enum PlaybackMode { Cyclic, Random }

    private AudioSource AudioSource
    {
        get
        {
            if (!_audioSource)
            {
                if (!s_audioPlayerContainer) s_audioPlayerContainer = AudioPlayerContainer.Instance;
                GameObject audioPlayerGameObj = new(nameof(AudioPlayer));
                audioPlayerGameObj.transform.SetParent(s_audioPlayerContainer.transform);
                _audioSource = audioPlayerGameObj.AddComponent<AudioSource>();
            }
            return _audioSource;
        }
    }
    private bool IsPlayable => (_audioClips.Length > 0) && (_interruptOnPlay || !IsPlaying);
    private bool IsPaused => AudioSource.clip && !AudioSource.isPlaying && AudioSource.time > 0f;
    private bool IsStopped => !AudioSource.clip && !AudioSource.isPlaying && AudioSource.time <= 0f;
    private bool IsPlaying => AudioSource.isPlaying;
    [ShowInInspector] private AudioClip CurrentAudioClip => AudioSource.clip;
    [ShowInInspector] private string Container => s_audioPlayerContainer != null ? s_audioPlayerContainer.name : null;
    [ShowInInspector, FoldoutGroup(nameof(Volume)), PropertyRange(0f, 1f), DisableIf(nameof(_randomizeVolume))]
    private float Volume
    {
        get => AudioSource.volume;
        set => AudioSource.volume = value;
    }
    [ShowInInspector, FoldoutGroup(nameof(Pitch)), PropertyRange(-3f, 3f), DisableIf(nameof(_randomizePitch))]
    private float Pitch
    {
        get => AudioSource.pitch;
        set => AudioSource.pitch = value;
    }

    [ButtonGroup]
    public void Play()
    {
        switch (_playbackMode)
        {
            case PlaybackMode.Cyclic:
                PlayCyclic();
                return;
            case PlaybackMode.Random:
                PlayRandom();
                return;
            default:
                return;
        }
    }

    [ButtonGroup, EnableIf(nameof(IsPaused))]
    private void Resume()
    {
        AudioSource.UnPause();
    }

    [ButtonGroup, EnableIf(nameof(IsPlaying))]
    private void Pause()
    {
        AudioSource.Pause();
    }

    [ButtonGroup, DisableIf(nameof(IsStopped))]
    private void Stop()
    {
        EndPlayback();
        _cyclicPlayer = GetCyclicPlayer();
    }

    private void PlayCyclic()
    {
        if (!IsPlayable) return;
        if (_cyclicPlayer is null) _cyclicPlayer = GetCyclicPlayer();
        if (!_cyclicPlayer.MoveNext())
        {
            _cyclicPlayer = GetCyclicPlayer();
            PlayCyclic();
        }
    }

    private void PlayRandom()
    {
        if (!IsPlayable) return;
        int randomIndex = Random.Range(0, _audioClips.Length - 1);
        StartPlayback(_audioClips[randomIndex]);
    }

    private void StartPlayback(AudioClip clip)
    {
        if (_randomizeVolume) AudioSource.volume = Random.Range(_volumeRange.x, _volumeRange.y);
        if (_randomizePitch) AudioSource.pitch = Random.Range(_pitchRange.x, _pitchRange.y);
        AudioSource.clip = clip;
        AudioSource.name = clip.name;
        AudioSource.Play();
    }

    private void EndPlayback()
    {
        AudioSource.Stop();
        AudioSource.name = nameof(AudioPlayer);
        AudioSource.clip = null;
    }

    private IEnumerator<AudioClip> GetCyclicPlayer()
    {
        foreach (AudioClip clip in _audioClips)
        {
            StartPlayback(clip);
            yield return clip;
        }
    }
}
