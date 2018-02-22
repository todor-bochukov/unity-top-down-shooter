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
        SetVolume(musicVolumeKey, GetMusicVolume());
        SetVolume(soundVolumeKey, GetSoundVolume());
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(musicVolumeKey, musicVolumeDefault);
    }

    public float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat(soundVolumeKey, soundVolumeDefault);
    }

    public void SetVolume(string key, float value)
    {
        foreach (var go in GameObject.FindGameObjectsWithTag(key))
        {
            foreach (var component in go.GetComponentsInChildren<AudioSource>())
            {
                component.volume = value;
            }
        }

        PlayerPrefs.SetFloat(key, value);
    }
}
