using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakFishEnemy : Enemy
{
    private bool movingRight = true;     // 是否向右移動
    private Vector3 startPosition;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
    }

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

    protected override void Patrol()
    {
        float patrolLimit = startPosition.x + (movingRight ? patrolDistance : -patrolDistance);

        // 左右來回移動
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime * (movingRight ? 1 : -1));

        // 達到巡邏邊界，反轉方向
        if ((movingRight && transform.position.x >= patrolLimit) || (!movingRight && transform.position.x <= patrolLimit))
        {
            movingRight = !movingRight;
            Flip();
        }
    }
}
