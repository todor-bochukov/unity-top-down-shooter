using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class Boomerang : MonoBehaviour
{
    public Vector2 velocity;

    private Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    private void FixedUpdate()
    {
        projectile.Body.velocity = transform.rotation * velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player && !player.Weapon && projectile.IsOldEnoughForPickup())
        {
            player.EquipWeapon(projectile.type);

            Destroy(gameObject);
        }

        var monster = collision.collider.GetComponent<Monster>();
        if (monster)
        {
            var spawner = monster.GetComponentInParent<MonsterSpawner>();
            spawner.KillMonster(monster);
        }

        var wall = collision.collider.GetComponent<Wall>();
        if (wall)
        {
            var contacts = collision.contacts;
            var reflected = Vector2.Reflect(velocity, contacts[0].normal);

            transform.Rotate(0, 0, Vector2.Angle(reflected, velocity));
        }
    }
}
