using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootItem : Item
{
    protected override void TriggerItem(GameObject player)
    {
        player.GetComponent<PlayerController>().GetShootItem();
    }
}
