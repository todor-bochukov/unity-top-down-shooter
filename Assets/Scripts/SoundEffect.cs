using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public bool playOnStart;
    public AudioClip clip;

    private void Start()
    {
        if (playOnStart)
        {
            Play();
        }
    }

    public void Play()
    {
        var audioControl = GameControl.FindAudioControl();
        var audioSource = GetComponentInParent<AudioSource>();

        audioControl.Play(audioSource, clip);
    }
}
