﻿using System.Collections;
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
        body.velocity = (player.Body.position - body.position).normalized * speed * control.TimeScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (player.Body == collision.rigidbody)
        {
            control.KillPlayer();
        }
    }
}