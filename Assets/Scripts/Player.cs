using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform playerLook;
    public Transform playerWeapon;
    public Transform projectileSpawnPoint;
    public Animator animator;

    [Header("Sounds")]
    public AudioClip pickUp;
    public AudioClip fire;

    private bool useMouse = false;

    private GameObject icon;

    public Rigidbody2D Body { get; private set; }
    public AudioSource AudioSource { get; private set; }
    public GameControl Control { get; private set; }

    public WeaponType Weapon { get; set; }

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();

        Control = GetComponentInParent<GameControl>();
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
        Body.velocity = direction * speed;

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

        Control.Audio.Play(AudioSource, pickUp);
    }

    private void UpdateWeapon()
    {
        if (!Weapon)
            return;

        if (Input.GetButton("Fire1"))
        {
            Instantiate(Weapon.projectile, projectileSpawnPoint.position, playerLook.rotation, Control.transform);

            Weapon = null;

            Destroy(icon);
            playerWeapon.gameObject.SetActive(false);

            Control.Audio.Play(AudioSource, fire);
        }
    }

    private float GetLookAngle()
    {
        if (useMouse)
        {
            var look = GetMouseWorldPosition() - Body.position;
            return Mathf.Atan2(look.y, look.x);
        }
        else
        {
            var look = new Vector2(Input.GetAxisRaw("Horizontal Look"), Input.GetAxisRaw("Vertical Look"));
            return look.SqrMagnitude() > 0.8f
                ? Mathf.Atan2(look.y, look.x)
                : Mathf.Deg2Rad * Body.rotation;
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
