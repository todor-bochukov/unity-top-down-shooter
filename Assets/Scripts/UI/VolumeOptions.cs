using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour
{
    public Slider musicVolume;
    public Slider soundVolume;

    private void Start()
    {
        var audioControl = GameControl.FindAudioControl();

        musicVolume.onValueChanged.AddListener(value => audioControl.SetMusicVolume(value));
        musicVolume.value = audioControl.GetMusicVolume();

        soundVolume.onValueChanged.AddListener(value => audioControl.SetSoundVolume(value));
        soundVolume.value = audioControl.GetSoundVolume();
    }

}
