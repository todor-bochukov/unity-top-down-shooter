using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;

    public GameObject deathPrefab;

    public Animator animator;

    public Rigidbody2D Body { get; private set; }
    private GameControl control;

    private void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        control = GetComponentInParent<GameControl>();
    }

    public void OnAI()
    {
        var player = FindObjectOfType<Player>();

        var target = Navigation.Instance.FindPath(Body.position, player.Body.position);

        Body.velocity = (target - Body.position).normalized * speed;

        animator.SetFloat("angle", Vector2.SignedAngle(Vector2.right, Body.velocity));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.rigidbody.GetComponent<Player>();
        if (player == null) return;

        control.KillPlayer();
    }
}
