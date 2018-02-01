using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public WeaponType type;
    public float minTimeToPickUp;

    public Rigidbody2D Body { get { return body; } }

    private Rigidbody2D body;

    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time;
        Debug.Log("startTime");
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (body.IsSleeping())
        {
            var control = GetComponentInParent<GameControl>();
            Instantiate(type.pickable, transform.position, transform.rotation, control.transform);

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();

        if (player && !player.Weapon && IsOldEnoughForPickup())
        {
            player.EquipWeapon(type);
            Destroy(gameObject);
        }
    }

    private bool IsOldEnoughForPickup()
    {
        if (spawnTime == 0)
            return false;

        Debug.Log(" s " + spawnTime + " t " + Time.time + " - " + (Time.time - spawnTime));

        return Time.time - spawnTime > minTimeToPickUp;
    }
}
