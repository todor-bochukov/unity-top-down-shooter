using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public WeaponType type;

    public Rigidbody2D Body { get { return body; } }

    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (body.IsSleeping())
        {
            var control = GetComponentInParent<GameControl>();
            Instantiate(type.pickable, transform.position, transform.rotation, control.transform);

            Destroy(gameObject);
        }
    }
}
