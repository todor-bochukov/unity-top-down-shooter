using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public WeaponType type;

    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (!player) return;
        if (player.Weapon) return;

        player.EquipWeapon(type);

        Destroy(gameObject);
    }
}
