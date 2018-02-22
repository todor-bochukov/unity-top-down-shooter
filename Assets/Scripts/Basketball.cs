using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class Basketball : MonoBehaviour
{
    public float throwStrength;

    private Projectile projectile;

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    private void Start()
    {
        projectile.Body.AddRelativeForce(Vector2.right * throwStrength, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player && !player.Weapon && projectile.IsOldEnoughForPickup())
        {
            player.EquipWeapon(projectile.type);

            Destroy(gameObject);
        }
        else if (player && player.Weapon)
        {
            var strength = Mathf.Max(0, throwStrength - projectile.Body.velocity.magnitude);
            projectile.Body.AddRelativeForce(Vector2.right * strength, ForceMode2D.Impulse);
        }

        var monster = collision.collider.GetComponent<Monster>();
        if (monster)
        {
            var spawner = monster.GetComponentInParent<MonsterSpawner>();
            spawner.KillMonster(monster);
        }
    }
}
