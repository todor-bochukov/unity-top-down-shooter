using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Boomerang : MonoBehaviour
{
    public Vector2 force;

    public Rigidbody2D Body { get { return body; } }

    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();

        body.AddRelativeForce(new Vector2(10, 0), ForceMode2D.Impulse);
        // body.AddTorque(-1, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        body.AddRelativeForce(new Vector2(0, -0.1f), ForceMode2D.Impulse);
        //body.AddRelativeForce(new Vector2(10, 0), ForceMode2D.Impulse);
        //body.AddTorque(100, ForceMode2D.Impulse);
    }
}
