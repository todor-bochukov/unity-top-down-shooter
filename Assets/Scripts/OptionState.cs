using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OptionState
{
    public bool isOn;
    public string text;
    public UnityEvent onOptionSet = new UnityEvent();

    public void Set()
    {
        if (onOptionSet != null)
            onOptionSet.Invoke();
    }
}
