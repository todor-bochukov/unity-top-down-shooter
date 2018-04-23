using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public bool wholeNumbers;
    public float minValue;
    public float maxValue;

    private void Start()
    {
        UnityAction<float> setValueToText = value => text.text = Convert(value).ToString();

        setValueToText(slider.value);

        slider.onValueChanged.AddListener(setValueToText);
    }

    private float Convert(float value)
    {
        Debug.Assert(slider.maxValue != slider.minValue);

        var sliderRange = slider.maxValue - slider.minValue;
        var range = maxValue - minValue;

        value = minValue + range * (value - slider.minValue) / sliderRange;

        if (wholeNumbers)
            value = Mathf.Round(value);

        return value;
    }
}
