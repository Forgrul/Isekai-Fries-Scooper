using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootItem : Item
{
    public int ammo = 10;

    protected override void TriggerItem(GameObject player)
    {
        player.GetComponent<PlayerController>().GetShootItem(ammo);
    }
}
