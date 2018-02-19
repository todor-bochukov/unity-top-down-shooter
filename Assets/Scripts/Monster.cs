using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;

    public GameObject deathPrefab;

    public Animator animator;

    private Rigidbody2D body;
    private GameControl control;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        control = GetComponentInParent<GameControl>();
    }

    private void FixedUpdate()
    {
        Player closestPlayer = null;
        foreach (var playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            var player = playerObject.GetComponent<Player>();
            if (!player) continue;

            if (!closestPlayer ||
                GetDistanceToPlayer(closestPlayer) > GetDistanceToPlayer(player))
            {
                closestPlayer = player;
            }
        }

        body.velocity = GetDirectionToPlayer(closestPlayer) * speed;

        animator.SetFloat("angle", Vector2.SignedAngle(Vector2.right, body.velocity));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.rigidbody.GetComponent<Player>();
        if (player == null) return;

        control.KillPlayer();
    }

    private Vector2 GetDirectionToPlayer(Player player)
    {
        return (player.Body.position - body.position).normalized;
    }

    private float GetDistanceToPlayer(Player player)
    {
        return (player.Body.position - body.position).magnitude;
    }
}
