using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponType : ScriptableObject
{
    public WeaponType[] ammoTypes = new WeaponType[0];

    public bool CanUse(WeaponType ammo)
    {
        foreach (var ammoType in ammoTypes)
        {
            if (ammoType == ammo)
                return true;
        }

        return false;
    }
}
