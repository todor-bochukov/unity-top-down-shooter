using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public OptionState state;
    public ToggleGroup group;

    public Toggle toggle;
    public Text text;
    public Text slash;

    private void Start()
    {
        toggle.group = group;

        if (state.isOn)
        {
            group.allowSwitchOff = false;
        }

        toggle.isOn = state.isOn;
        text.text = state.text;

        slash.enabled = !IsLastOnLine();
    }

    private void Update()
    {
        slash.enabled = !IsLastOnLine();
    }

    public void OnToggleValueChanged(bool isOn)
    {
        if (!isOn)
            return;

        group.allowSwitchOff = false;

        state.Set();
    }

    private bool IsLastOnLine()
    {
        var parent = transform.parent;

        var index = transform.GetSiblingIndex();
        var nextIndex = index + 1;
        if (nextIndex < parent.childCount)
        {
            var sibling = parent.GetChild(nextIndex);

            return transform.position.y != sibling.position.y;
        }

        return true;
    }

}
