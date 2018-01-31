using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponType type;

    private GameObject icon;

    private void Start()
    {
    }

    private void Update()
    {
        if (type == null)
        {
            if (icon != null)
            {
                Destroy(icon);
            }
        }
        else
        {
            if (icon == null)
            {
                icon = Instantiate(type.icon, transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public void Reposition(Vector2 position)
    {
    }
}

