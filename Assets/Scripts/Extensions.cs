using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class ResolutionExt
{
    public static AspectRatio GetKnownAspectRatio(this Resolution resolution)
    {
        return new AspectRatio
        {
            width = resolution.width,
            height = resolution.height,
        }.ToKnownAspectRatio();
    }
}
