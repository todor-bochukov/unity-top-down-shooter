using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private bool permitSkip;

    private void Update()
    {
        if (permitSkip && Input.anyKeyDown)
        {
            OnGameOverAnimationEnd();
        }
    }

    public void OnGameOverVisible()
    {
        permitSkip = true;
    }

    public void OnGameOverAnimationEnd()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
