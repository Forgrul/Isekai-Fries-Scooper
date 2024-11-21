using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : Enemy
{
    protected override void Patrol()
    {
        return;
    }
    protected override void Fire()
    {
        Vector2 direction = new Vector2(-1, 0);
        float spawnDistance = 0.8f;
        Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;

        GameObject b = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, 180));
        Bullet bulletScript = b.GetComponent<Bullet>();

        bulletScript.SetDirection(direction);
    }
}
