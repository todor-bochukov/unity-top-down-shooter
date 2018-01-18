using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public UIControl ui;
    public float timeScaleSmoothTime;

    public event Action onRestart;
    public float TimeScale { get; private set; }

    private float timeScaleVelocity;

    private void Update()
    {
        bool inUI = ui.gameObject.activeSelf;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleUI(!inUI);
        }
        else if (!inUI)
        {
            TimeScale = Mathf.SmoothDamp(TimeScale, 1f, ref timeScaleVelocity, timeScaleSmoothTime);
        }
    }

    public void KillPlayer()
    {
        if (onRestart != null)
            onRestart();
    }

    public void ToggleUI(bool openUI)
    {
        ui.gameObject.SetActive(openUI);

        // Stop game / restart timeScale timer.
        TimeScale = 0f;
        timeScaleVelocity = 0f;
    }
}
