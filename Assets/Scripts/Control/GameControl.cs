﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioControl))]
public class GameControl : MonoBehaviour
{
    public UIControl ui;
    public HUDControl hud;
    public float timeScaleSmoothTime;

    private float timeScaleVelocity;

    private uint kills;

    public AudioControl Audio { get; private set; }

    private void Awake()
    {
        Audio = GetComponent<AudioControl>();
    }

    private void Update()
    {
        bool inUI = ui.gameObject.activeSelf;

        if (inUI)
        {
            Time.timeScale = 0f;
            timeScaleVelocity = 0f;
        }
        else
        {
            Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1f, ref timeScaleVelocity, timeScaleSmoothTime, 100, Time.unscaledDeltaTime);

            if (Input.GetButtonDown("Cancel"))
            {
                ui.gameObject.SetActive(true);
            }
        }
    }

    public void KillMonster()
    {
        ++kills;
        hud.SetKills(kills);
    }

    public void KillPlayer()
    {
        ui.gameObject.SetActive(true);
        ui.GameOver();
    }

    public static AudioControl FindAudioControl()
    {
        return FindComponent<AudioControl>();
    }
    public static T FindComponent<T>()
    {
        return GameObject.FindWithTag("GameController").GetComponent<T>();
    }
}
