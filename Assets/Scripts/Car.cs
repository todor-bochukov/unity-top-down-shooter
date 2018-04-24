using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class Car : MonoBehaviour
{
    public float speed;
    public float speedUpPerKill;

    [Range(0, 90)]
    public float minAngleToRotate;

    private Projectile projectile;

    private RaycastHit2D[] raycastHits = new RaycastHit2D[1000];

    private void Awake()
    {
        projectile = GetComponent<Projectile>();
    }

    private void FixedUpdate()
    {
        var targetVelocity = transform.rotation * Vector2.right * speed;

        var bestVelocity = targetVelocity;

        var bestRotation = transform.rotation;
        var bestHitCount = projectile.Body.Cast(bestVelocity, raycastHits);
        var bestHitDistance = FindNearestWall();

        Debug.DrawLine(transform.position, transform.position + targetVelocity * bestHitDistance / targetVelocity.magnitude, Color.cyan);

        if (bestHitDistance < 1)
        {
            for (var angle = 0; angle < 360; angle += 45)
            {
                var rotation = Quaternion.Euler(0, 0, angle);
                var velocity = rotation * Vector2.right * speed;

                if (Vector2.Angle(targetVelocity, velocity) > 60)
                {
                    Debug.DrawLine(transform.position, transform.position + velocity, Color.red);
                    continue;
                }

                int hitCount = projectile.Body.Cast(velocity, raycastHits);

                var hitDistance = FindNearestWall();

                Debug.DrawLine(transform.position, transform.position + velocity * hitDistance / velocity.magnitude, Color.white);

                if (hitDistance > bestHitDistance)
                {
                    bestRotation = rotation;
                    bestHitDistance = hitDistance;
                    bestVelocity = velocity;
                }
            }
        }

        if (bestHitDistance == Mathf.Infinity)
        {
            // out of stage
            projectile.SpawnPickable();
            return;
        }

        transform.rotation = bestRotation;

        projectile.Body.velocity = bestVelocity;
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

            speed += speedUpPerKill;
        }

        var wall = collision.collider.GetComponent<Wall>();
        if (wall)
        {
            projectile.SpawnPickable();
        }
    }

    private float FindNearestWall()
    {
        var distance = Mathf.Infinity;
        foreach (var hit in raycastHits)
        {
            if (!hit.collider)
                continue;

            var wall = hit.collider.GetComponent<Wall>();
            if (!wall)
                continue;

            if (hit.distance < distance)
            {
                distance = hit.distance;
            }
        }

        return distance;
    }
}
