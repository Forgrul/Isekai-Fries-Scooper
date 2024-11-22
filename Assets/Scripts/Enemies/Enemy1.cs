using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    [Header("Move Settings")]
    public float minMoveCD = 0.5f;
    public float maxMoveCD = 2f;
    public bool canFly = false;

    // Patrol
    float stopMoveTimer;
    Vector3 startPosition;
    Vector3 targetPosition;

    protected override void Start()
    {
        base.Start();
        stopMoveTimer = Random.Range(minMoveCD, maxMoveCD);
        if(canFly)
            rb.gravityScale = 0;
        
        startPosition = transform.position;
        targetPosition = new Vector3(startPosition.x + Random.Range(-patrolDistance, patrolDistance), startPosition.y, startPosition.z);
    }

    protected override void Patrol()
    {
        stopMoveTimer -= Time.deltaTime;
        if(stopMoveTimer >= 0)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if(transform.position == targetPosition)
        {
            stopMoveTimer = Random.Range(minMoveCD, maxMoveCD);

            targetPosition = new Vector3(startPosition.x + Random.Range(-patrolDistance, patrolDistance), transform.position.y, transform.position.z);
        }
    }



    protected override void Fire()
    {
        StartCoroutine(FireSpiral());
    }

    IEnumerator FireSpiral()
    {
        int fireCount = 20;
        for(int i = 0; i < fireCount; i++)
        {
            float angle = 360f / fireCount * i;

            for(int j = 0; j < 2; j++)
            {
                GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
                Bullet bulletScript = b.GetComponent<Bullet>();
                bulletScript.SetDirectionAngle(angle);
                angle += 180f;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
