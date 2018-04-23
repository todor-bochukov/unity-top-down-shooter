using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AspectRatio
{
    public int width;
    public int height;

    public float Ratio
    {
        get
        {
            if (height == 0)
                return 0;

            return width / (float)height;
        }
    }

    public static AspectRatio[] knownAspectRatios = new[] {
        new AspectRatio { width = 5,  height = 4, },
        new AspectRatio { width = 4,  height = 3, },
        new AspectRatio { width = 16, height = 9, },
        new AspectRatio { width = 16, height = 10, },
        new AspectRatio(),
    };

    //

    public override string ToString()
    {
        if (Mathf.Approximately(Ratio, 0))
            return "Other";

        return width + ":" + height;
    }

    public AspectRatio ToKnownAspectRatio()
    {
        foreach (var knownAspectRatio in knownAspectRatios)
        {
            if (this == knownAspectRatio)
                return knownAspectRatio;
        }

        return new AspectRatio();
    }

    //

    public override bool Equals(object other)
    {
        if (other == null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Equals((AspectRatio)other);
    }

    public bool Equals(AspectRatio other)
    {
        return Mathf.Approximately(Ratio, other.Ratio);
    }

    public override int GetHashCode()
    {
        return width ^ height;
    }

    public static bool operator ==(AspectRatio left, AspectRatio right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AspectRatio left, AspectRatio right)
    {
        return !left.Equals(right);
    }
}
