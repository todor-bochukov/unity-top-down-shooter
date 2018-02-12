using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAnimator : MonoBehaviour
{
    public Animator animator;
    public string boolParameterName;

    public void OnToggleValueChanged(bool isOn)
    {
        animator.SetBool(boolParameterName, isOn);
    }
}
