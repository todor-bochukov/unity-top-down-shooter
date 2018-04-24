using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public WeaponType type;
    public float minTimeToPickUp;

    public Rigidbody2D Body { get; private set; }
    public float SpawnTime { get; private set; }

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SpawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (Body.IsSleeping())
        {
            SpawnPickable();
        }
    }

    public void SpawnPickable()
    {
        var control = GetComponentInParent<GameControl>();
        Instantiate(type.pickable, transform.position, transform.rotation, control.transform);

        Destroy(gameObject);
    }

    public bool IsOldEnoughForPickup()
    {
        if (SpawnTime == 0) return false;

        return Time.time - SpawnTime > minTimeToPickUp;
    }
}
