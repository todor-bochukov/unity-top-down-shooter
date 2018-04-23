using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CancelButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            button.onClick.Invoke();
        }
    }
}
