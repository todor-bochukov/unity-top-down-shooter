using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleAnimator : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        animator.SetFloat("angle", transform.rotation.eulerAngles.z);
    }
}
