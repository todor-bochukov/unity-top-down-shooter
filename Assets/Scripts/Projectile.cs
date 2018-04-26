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

    private bool destroyed = false;

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

    public void Despawn()
    {
        if (destroyed) return;

        destroyed = true;

        Destroy(gameObject);
    }

    public void SpawnPickable()
    {
        if (destroyed) return;

        var control = GetComponentInParent<GameControl>();
        Instantiate(type.pickable, transform.position, transform.rotation, control.transform);

        Despawn();
    }

    public bool IsOldEnoughForPickup()
    {
        if (SpawnTime == 0) return false;

        return Time.time - SpawnTime > minTimeToPickUp;
    }
}
