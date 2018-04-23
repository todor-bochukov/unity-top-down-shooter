using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleAnimator : MonoBehaviour
{
    public Animator animator;
    public string boolParameterName;

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void OnEnable()
    {
        animator.SetBool(boolParameterName, toggle.isOn);
    }

    public void OnToggleValueChanged(bool isOn)
    {
        animator.SetBool(boolParameterName, isOn);
    }
}
