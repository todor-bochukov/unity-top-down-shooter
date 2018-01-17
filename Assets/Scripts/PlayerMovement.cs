using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 1.0f;

    private new Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rigidbody2D.velocity = direction * speed;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var look = GetMouseWorldPosition() - rigidbody2D.position;
        rigidbody2D.rotation = Mathf.Rad2Deg * Mathf.Atan2(look.y, look.x);
    }

    private Vector2 GetMouseWorldPosition()
    {
        var camera = Camera.main;

        if (camera.orthographic)
        {
            return camera.ScreenToWorldPoint(Input.mousePosition);
        }

        var ray = camera.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (new Plane(Vector3.back, 0).Raycast(ray, out distance))
            return ray.GetPoint(distance);

        return new Vector2();
    }
}
