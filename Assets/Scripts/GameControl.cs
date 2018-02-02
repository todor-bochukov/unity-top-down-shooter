using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public UIControl ui;
    public float timeScaleSmoothTime;

    private float timeScaleVelocity;

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
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleUI(!inUI);
        }
    }

    public void KillPlayer()
    {
        ToggleUI(true);
        ui.GameOver();
    }

    public void ToggleUI(bool openUI)
    {
        ui.gameObject.SetActive(openUI);

    }
}
