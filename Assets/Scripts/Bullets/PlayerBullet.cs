using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if(other.gameObject.CompareTag("Enemy"))
        {
            // Damage enemy
            other.gameObject.GetComponent<Enemy>().GetHit();
            Destroy(gameObject);
        }
    }
}
