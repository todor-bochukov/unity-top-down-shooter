using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour
{
    public AudioControl audioControl;
    public Slider musicVolume;
    public Slider soundVolume;

    private void Start()
    {
        musicVolume.onValueChanged.AddListener(CreateSetVolumeFn(AudioControl.musicVolumeKey));
        musicVolume.value = audioControl.GetMusicVolume();

        soundVolume.onValueChanged.AddListener(CreateSetVolumeFn(AudioControl.soundVolumeKey));
        soundVolume.value = audioControl.GetSoundVolume();
    }

    private UnityAction<float> CreateSetVolumeFn(string key)
    {
        return value => audioControl.SetVolume(key, value);
    }
}
