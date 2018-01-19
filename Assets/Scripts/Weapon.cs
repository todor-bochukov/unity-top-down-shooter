using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState
{
    Grounded,
    Carried,
    Flying,
}

[RequireComponent(typeof(Rigidbody2D))]
public class Weapon : MonoBehaviour
{
    public WeaponType type;
    public float force;

    public Rigidbody2D Body { get { return body; } }
    public Weapon Ammo { get; set; }
    public WeaponState State { get; set; }

    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (State == WeaponState.Flying)
        {
            body.velocity *= 0.9f;

            if (body.velocity.magnitude < 0.1f)
            {
                body.velocity = new Vector2();
                State = WeaponState.Grounded;
            }

            if (Ammo) Ammo.Reposition(body.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (State)
        {
            case WeaponState.Grounded:
                TryPickUp(collision);
                break;
            case WeaponState.Flying:
                TryHit(collision);
                break;
        }
    }

    public void Reposition(Vector2 position)
    {
        transform.position = position;

        if (Ammo)
        {
            Ammo.Reposition(position);
        }
    }

    private void TryPickUp(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (!player) return;

        if (!player.Weapon)
        {
            player.Weapon = this;
            State = WeaponState.Carried;
        }
        else if (!player.Weapon.Ammo)
        {
            if (player.Weapon.type.CanUse(type))
            {
                player.Weapon.Ammo = this;
                State = WeaponState.Carried;
            }
            else if (type.CanUse(player.Weapon.type))
            {
                Ammo = player.Weapon;
                player.Weapon = this;
                State = WeaponState.Carried;
            }
        }
    }

    private void TryHit(Collider2D collision)
    {
        var monster = collision.GetComponent<Monster>();
        if (!monster) return;

        var spawner = monster.GetComponentInParent<MonsterSpawner>();
        spawner.KillMonster(monster);
    }
}
