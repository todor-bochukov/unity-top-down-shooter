using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boomerang : MonoBehaviour
{
    public Vector2 velocity;

    public Rigidbody2D Body { get { return body; } }

    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.velocity = transform.rotation * velocity;
    }

    private void FixedUpdate()
    {
        body.velocity = transform.rotation * velocity;
    }
}
