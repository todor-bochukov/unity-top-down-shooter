using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public const string musicVolumeKey = "Music";
    public const string soundVolumeKey = "Sound";

    public float musicVolumeDefault;
    public float soundVolumeDefault;

    void Awake()
    {
        SetMusicVolume(GetMusicVolume());
        SetSoundVolume(GetSoundVolume());
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(musicVolumeKey, musicVolumeDefault);
    }

    public void SetMusicVolume(float volume)
    {
        foreach (var audioSource in FindObjectsOfType<AudioSource>())
        {
            if (audioSource.tag == musicVolumeKey)
            {
                audioSource.volume = volume;
            }
        }

        PlayerPrefs.SetFloat(musicVolumeKey, volume);
    }

    public float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat(soundVolumeKey, soundVolumeDefault);
    }

    public void SetSoundVolume(float value)
    {
        foreach (var audioSource in FindObjectsOfType<AudioSource>())
        {
            if (audioSource.tag != musicVolumeKey)
            {
                audioSource.volume = value;
            }
        }

        PlayerPrefs.SetFloat(soundVolumeKey, value);
    }

    public void Play(AudioSource source, AudioClip clip)
    {
        source.clip = clip;

        Play(source);
    }

    public void Play(AudioSource source)
    {
        if (source.tag == musicVolumeKey)
        {
            source.volume = GetMusicVolume();
        }
        else
        {
            source.volume = GetSoundVolume();
        }

        source.Play();
    }
}
