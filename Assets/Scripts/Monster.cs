using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;

    private Player player;
    private Rigidbody2D body;
    private GameControl control;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        control = GetComponentInParent<GameControl>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Assert(player != null);
    }

    private void FixedUpdate()
    {
        if (control.TimeScale == 0f)
            return;

        body.velocity = (player.Body.position - body.position).normalized * speed * control.TimeScale;
    }
}
