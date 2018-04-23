using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsOptions : MonoBehaviour
{
    public OptionGroup displayModeGroup;
    public OptionGroup aspectRatioGroup;
    public OptionGroup resolutionGroup;
    public OptionGroup refreshRateGroup;

    private AspectRatio currentAspectRatio = new AspectRatio
    {
        width = Screen.width,
        height = Screen.height,
    };

    //

    private void Start()
    {
        currentAspectRatio = currentAspectRatio.ToKnownAspectRatio();

        displayModeGroup.options = ListDisplayModes();
        displayModeGroup.OnOptionsChanged();

        aspectRatioGroup.options = ListAspectRatios();
        aspectRatioGroup.OnOptionsChanged();

        UpdateResolutions();
        UpdateRefreshRates();
    }

    public void SetFulscreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    private void UpdateResolutions()
    {
        resolutionGroup.options = ListResolutions();
        resolutionGroup.OnOptionsChanged();
    }

    private void UpdateRefreshRates()
    {
        refreshRateGroup.options = ListRefreshRates();
        refreshRateGroup.OnOptionsChanged();
    }

    //

    private OptionState[] ListDisplayModes()
    {
        var onOptionSet = new UnityEngine.Events.UnityEvent();
        onOptionSet.AddListener(() => Screen.fullScreen = !Screen.fullScreen);

        return new[] {
            new OptionState()
            {
                isOn = !Screen.fullScreen,
                text = "Windowed",
                onOptionSet = onOptionSet,
            },
            new OptionState()
            {
                isOn = Screen.fullScreen,
                text = "Fullscreen",
                onOptionSet = onOptionSet,
            },
        };
    }

    private OptionState[] ListAspectRatios()
    {
        var result = new List<OptionState>();

        foreach (var aspectRatio in AspectRatio.knownAspectRatios)
        {
            var option = new OptionState
            {
                isOn = aspectRatio == currentAspectRatio,
                text = aspectRatio.ToString(),
            };

            option.onOptionSet.AddListener(() =>
            {
                currentAspectRatio = aspectRatio;
                UpdateResolutions();
            });

            result.Add(option);
        }

        return result.ToArray();
    }

    private OptionState[] ListResolutions()
    {
        var result = new List<OptionState>();

        var resolutions = new List<string>();

        foreach (var resolution in Screen.resolutions)
        {
            if (currentAspectRatio != resolution.GetKnownAspectRatio())
                continue;

            var name = resolution.width + "x" + resolution.height;

            // filter out multiple refresh rates.
            if (resolutions.Contains(name))
                continue;

            resolutions.Add(name);

            // add resolution to result.
            var option = new OptionState
            {
                isOn = resolution.width == Screen.width && resolution.height == Screen.height,
                text = name,
            };

            option.onOptionSet.AddListener(() =>
            {
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
                UpdateRefreshRates();
            });

            result.Add(option);
        }

        return result.ToArray();
    }

    private OptionState[] ListRefreshRates()
    {
        var result = new List<OptionState>();

        foreach (var resolution in Screen.resolutions)
        {
            if (Screen.width != resolution.width || Screen.height != resolution.height)
                continue;

            var refreshRate = resolution.refreshRate;

            var option = new OptionState
            {
                isOn = Screen.currentResolution.refreshRate == refreshRate,
                text = refreshRate.ToString(),
            };

            option.onOptionSet.AddListener(() =>
                Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen, refreshRate));

            result.Add(option);
        }

        // Add the current refresh rate in case the current resolution is weird.
        if (result.Count == 0)
        {
            var option = new OptionState
            {
                isOn = true,
                text = Screen.currentResolution.refreshRate.ToString(),
            };

            result.Add(option);
        }

        return result.ToArray();
    }
}
