using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LayerMaskŠg’£ƒNƒ‰ƒX
/// </summary>
public static class LayerMaskExtensions
{
    public static bool Contains(this LayerMask self, int layerId)
    {
        return ((1 << layerId) & self) != 0;
    }

    public static bool Contains(this LayerMask self, string layerName)
    {
        int layerId = LayerMask.NameToLayer(layerName);
        return ((1 << layerId) & self) != 0;
    }

    public static LayerMask Add(this LayerMask self, LayerMask layerId)
    {
        return self | (1 << layerId);
    }

    public static LayerMask Toggle(this LayerMask self, LayerMask layerId)
    {
        return self ^ (1 << layerId);
    }

    public static LayerMask Remove(this LayerMask self, LayerMask layerId)
    {
        return self & ~(1 << layerId);
    }
}