using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{
    public Animator animator;
    public Text killsText;

    void Start()
    {
        SetKills(0);
    }

    public void SetKills(uint kills)
    {
        killsText.text = kills.ToString();
        if (kills > 0)
        {
            animator.SetTrigger("Kill");
        }
    }
}
