using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject main;
    public GameObject gameOver;

    public AudioSource backgroundMusic;

    private void Start()
    {
        // Scene restarted on game over. Do not pause.
        if (Time.unscaledTime > 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif // UNITY_EDITOR
    }

    public void GameOver()
    {
        backgroundMusic.Stop();

        main.SetActive(false);
        gameOver.SetActive(true);
    }
}
