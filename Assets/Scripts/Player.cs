using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform playerLook;

    private Rigidbody2D body;
    private GameControl control;

    private bool useMouse = false;

    public Rigidbody2D Body { get { return body; } }
    public Weapon Weapon { get; set; }

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        control = GetComponentInParent<GameControl>();
    }

    private void FixedUpdate()
    {
        UpdateMovement();

        UpdateMode();
        UpdateLook();

        // Weapons.
        if (Weapon)
        {
            Weapon.Reposition(body.position);

            var force = playerLook.rotation * Vector2.right;

            var fire1 = Input.GetButtonDown("Fire1");
            var fire2 = Input.GetButtonDown("Fire2");

            if (Weapon.Ammo && fire1)
            {
                force *= 2 * (Weapon.force + Weapon.Ammo.force);
                Weapon.Ammo.State = WeaponState.Flying;
                Weapon.Ammo.Body.velocity = force;
                Weapon.Ammo.Body.rotation = Mathf.Rad2Deg * Mathf.Atan2(force.y, force.x);

                Weapon.Ammo = null;
            }
            else if (fire1 || fire2)
            {
                force *= Weapon.force;
                Weapon.State = WeaponState.Flying;
                Weapon.Body.velocity = force;
                Weapon.Body.rotation = Mathf.Rad2Deg * Mathf.Atan2(force.y, force.x);
                Weapon = null;
            }
        }

    }

    private void OnEnable()
    {
        var control = GetComponentInParent<GameControl>();
        if (control != null)
            control.onRestart += Restart;
    }

    private void OnDisable()
    {
        var control = GetComponentInParent<GameControl>();
        if (control != null)
            control.onRestart += Restart;
    }

    void Restart()
    {
        body.position = new Vector2();
        body.velocity = new Vector2();
    }

    private void UpdateMovement()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        body.velocity = direction * speed * control.TimeScale;
    }

    private void UpdateMode()
    {
        var mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        var joystick = new Vector2(Input.GetAxisRaw("Horizontal Look"), Input.GetAxisRaw("Vertical Look"));

        if (mouse.SqrMagnitude() > Mathf.Epsilon)
            useMouse = true;
        else if (joystick.SqrMagnitude() > Mathf.Epsilon)
            useMouse = false;
    }

    private void UpdateLook()
    {
        if (control.TimeScale == 0)
            return;

        playerLook.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * GetLookAngle());
    }

    private float GetLookAngle()
    {
        if (useMouse)
        {
            var look = GetMouseWorldPosition() - body.position;
            return Mathf.Atan2(look.y, look.x);
        }
        else
        {
            var look = new Vector2(Input.GetAxisRaw("Horizontal Look"), Input.GetAxisRaw("Vertical Look"));
            return look.SqrMagnitude() > 0.8f
                ? Mathf.Atan2(look.y, look.x)
                : Mathf.Deg2Rad * body.rotation;
        }
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
