using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    public void OnButtonClick(AudioClip clip)
    {
        var audioControl = GameControl.FindAudioControl();
        var audioSource = GetComponentInParent<AudioSource>();

        audioControl.Play(audioSource, clip);
    }
}
