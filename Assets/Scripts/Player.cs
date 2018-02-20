﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform playerLook;
    public Transform playerWeapon;
    public Transform projectileSpawnPoint;
    public Animator animator;

    private Rigidbody2D body;

    private bool useMouse = false;

    private GameObject icon;

    public Rigidbody2D Body { get { return body; } }
    public WeaponType Weapon { get; set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateMovement();

        UpdateMode();
        UpdateLook();

        UpdateWeapon();
    }

    private void UpdateMovement()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        body.velocity = direction * speed;

        animator.SetFloat("Move X", Mathf.Round(direction.x));
        animator.SetFloat("Move Y", Mathf.Round(direction.y));
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
        if (Time.timeScale == 0)
            return;

        float angle = Mathf.Rad2Deg * GetLookAngle();
        playerLook.rotation = Quaternion.Euler(0, 0, angle);
        animator.SetFloat("angle", angle);
    }

    public void EquipWeapon(WeaponType weapon)
    {
        Weapon = weapon;

        playerWeapon.gameObject.SetActive(true);
        icon = Instantiate(Weapon.icon, playerWeapon);
    }

    private void UpdateWeapon()
    {
        if (!Weapon)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            var control = GetComponentInParent<GameControl>();
            Instantiate(Weapon.projectile, projectileSpawnPoint.position, playerLook.rotation, control.transform);

            Weapon = null;

            Destroy(icon);
            playerWeapon.gameObject.SetActive(false);
        }
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
