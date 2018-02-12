using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionGroup : MonoBehaviour
{
    public Option optionPrefab;

    public ToggleGroup group;
    public OptionState[] options = new OptionState[0];

    private List<Option> oldOptions = new List<Option>();

    public void OnOptionsChanged()
    {
        oldOptions.ForEach(option => Destroy(option.gameObject));
        oldOptions.Clear();

        group.allowSwitchOff = true;

        foreach (var optionState in options)
        {
            var option = Instantiate(optionPrefab, transform);
            oldOptions.Add(option);

            option.state = optionState;
            option.group = group;
        }
    }
}
