using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakFishEnemy : Enemy
{
    protected override void Fire()
    {
        int bulletCount = 6; // 六邊形的 6 顆子彈
        float angleStep = 360f / bulletCount; // 每顆子彈的角度間隔
        float spawnDistance = 0.7f; // 生成位置偏移距離

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            
            Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;
            GameObject b = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
            Bullet bulletScript = b.GetComponent<Bullet>();

            bulletScript.SetDirectionAngle(angle);
        }
    }
}
