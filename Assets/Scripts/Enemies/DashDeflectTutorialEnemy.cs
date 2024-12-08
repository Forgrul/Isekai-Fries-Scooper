using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDeflectTutorialEnemy : Enemy
{
    protected override void Patrol()
    {
        return;
    }
    protected override void Fire()
    {
        StartCoroutine(ShootMany());
    }

    IEnumerator ShootMany()
    {
        Vector2 direction = new Vector2(-1, 0);
        float spawnDistance = 0.8f;
        Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;

        for(int i = 0; i < 6; i++)
        {
            GameObject b = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, 180));
            Bullet bulletScript = b.GetComponent<Bullet>();

            bulletScript.SetDirection(direction);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
