using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _musicSource;
    private AudioSource _soundFXSource;

    internal override void Awake()
    {
        base.Awake();
        AudioSource[] childAudioSources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audioSource in childAudioSources)
        {
            if (!_musicSource && audioSource.name.ToLower().Contains("music"))
                _musicSource = audioSource;
            if (!_soundFXSource && audioSource.name.ToLower().Contains("sound"))
                _soundFXSource = audioSource;
        }
        if (!_musicSource)
        {
            _musicSource = CreateAudioSource("Music");
            Debug.LogWarning($"Could not find any child {nameof(AudioSource)} named for Music. New one is created.");
        }
        if (!_soundFXSource)
        {
            _soundFXSource = CreateAudioSource("SoundFX");
            Debug.LogWarning($"Could not find any child {nameof(AudioSource)} named for Sound. New one is created.");
        }
    }

    private AudioSource CreateAudioSource(string name)
    {
        GameObject audioSrcGameObj = new(name);
        audioSrcGameObj.transform.SetParent(transform);
        return audioSrcGameObj.AddComponent<AudioSource>();
    }

    private void Play(AudioSource audioSrc, AudioClip clip, bool oneshot = false, bool loop = false, float volume = -1f, float pitch = -1f)
    {
        audioSrc.clip = clip;
        audioSrc.loop = loop;
        if (volume > 0f) audioSrc.volume = volume;
        if (pitch > 0f) audioSrc.pitch = pitch;
        if (oneshot) audioSrc.PlayOneShot(clip);
        else audioSrc.Play();
    }

    public void PlayMusic(AudioClip musicClip, bool loop = false, float volume = -1f, float pitch = -1f) 
        => Play(_musicSource, musicClip, oneshot: false, loop, volume, pitch);

    public void PlaySoundEffect(AudioClip soundFXClip, bool loop = false, float volume = -1f, float pitch = -1f) 
        => Play(_soundFXSource, soundFXClip, oneshot: true, loop, volume, pitch);

    public void PlayMusicLocally(AudioSource localAudioSrc, AudioClip musicClip, bool loop = false, float volume = -1f, float pitch = -1f)
        => Play(localAudioSrc, musicClip, oneshot: false, loop, volume, pitch);

    public void PlaySoundEffectLocally(AudioSource localAudioSrc, AudioClip soundFXClip, bool loop = false, float volume = -1f, float pitch = -1f)
       => Play(localAudioSrc, soundFXClip, oneshot: true, loop, volume, pitch);
}
