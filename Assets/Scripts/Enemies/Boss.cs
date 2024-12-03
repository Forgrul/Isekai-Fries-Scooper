using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Movement Settings")]
    public float patrolDistance = 10f;
    public float minMoveCD = 0.5f;
    public float maxMoveCD = 2f;

    // [Header("Fire Settings")]
    // public GameObject slowBulletPrefab;
    // public GameObject fastBulletPrefab;

    [Header("Spiral Attack")]
    public int spiralFireCount = 20;

    [Header("Diverge Attack")]
    public int divergeWaveCount = 5;
    public float divergeWaveInterval = 0.5f;
    public int divergeBulletCount = 12;

    [Header("Upward Attack")]
    public int upwardWaveCount = 5;
    public float upwardWaveInterval = 0.5f;
    public int upwardBulletCount = 20;

    delegate IEnumerator Attack();

    // Patrol
    float stopMoveTimer;
    Vector3 startPosition;
    Vector3 targetPosition;

    public GameObject WinPanel;

    protected override void Start()
    {
        base.Start();
        stopMoveTimer = Random.Range(minMoveCD, maxMoveCD);
        
        startPosition = transform.position;
        targetPosition = new Vector3(startPosition.x + Random.Range(-patrolDistance, patrolDistance), transform.position.y, transform.position.z);
    }

    protected override void Update()
    {
        Patrol();

        fireTimer -= Time.deltaTime;

        // Only fire after stop moving
        if (fireTimer <= 0 && stopMoveTimer >= 0)
        {
            Fire();
            // this shouldn't be hardcoded...anyway
            float maxFireTime = 3f;
            // prevent it moves while firing
            stopMoveTimer = Mathf.Max(maxFireTime, stopMoveTimer);
            fireTimer = fireInterval; // 重置計時器
        }
    }

    // This is the same as Enemy1. Need to change later
    protected override void Patrol()
    {
        stopMoveTimer -= Time.deltaTime;
        if(stopMoveTimer >= 0)
            return;

        float scale = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(targetPosition.x > transform.position.x ? scale : -scale, scale, scale);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if(transform.position == targetPosition)
        {
            stopMoveTimer = Random.Range(minMoveCD, maxMoveCD);

            targetPosition = new Vector3(startPosition.x + Random.Range(-patrolDistance, patrolDistance), transform.position.y, transform.position.z);
        }
    }
    
    protected override void Fire()
    {
        Attack[] attacks = { SpiralAttack, DivergeAttack, UpwardAttack };
        int index = Random.Range(0, attacks.Length);
        StartCoroutine(attacks[index]());
    }

    IEnumerator SpiralAttack()
    {
        float spawnDistance = 1f;
        for(int i = 0; i < spiralFireCount; i++)
        {
            float angle = 360f / spiralFireCount * i;

            for(int j = 0; j < 2; j++)
            {
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;
                GameObject b = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
                Bullet bulletScript = b.GetComponent<Bullet>();
                bulletScript.SetDirection(direction);
                angle += 180f;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator DivergeAttack()
    {
        float spawnDistance = 1f;
        float initPhase = Random.Range(0f, 360f);
        for(int i = 0; i < divergeWaveCount; i++)
        {
            float phase = initPhase + (360f / divergeBulletCount) / 2 * i;
            for(int j = 0; j < divergeBulletCount; j++)
            {
                float angle = 360f / divergeBulletCount * j + phase;
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;
                GameObject b = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
                Bullet bulletScript = b.GetComponent<Bullet>();
                bulletScript.SetDirection(direction);
            }

            yield return new WaitForSeconds(divergeWaveInterval);
        }
    }

    IEnumerator UpwardAttack()
    {
        float spawnDistance = 2f;
        for(int i = 0; i < upwardWaveCount; i++)
        {
            for(int j = 0; j < upwardBulletCount; j++)
            {
                float angle = Random.Range(60f, 120f);

                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;

                GameObject b = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
                Bullet bulletScript = b.GetComponent<Bullet>();
                bulletScript.SetDirection(direction);
            }

            yield return new WaitForSeconds(upwardWaveInterval);
        }
    }

    void OnDestroy()
    {
        Time.timeScale = 0;
        Transform statusUI = GameObject.Find("StatusUI")?.transform;
        Transform LevelTimerTransform = statusUI?.Find("LevelTimer");
        GameObject LevelTimer = LevelTimerTransform?.gameObject;
        LevelTimer?.GetComponent<LevelTimer>().ShowCompletionTime();
        if(WinPanel != null)
            WinPanel.SetActive(true);
    } 
}
