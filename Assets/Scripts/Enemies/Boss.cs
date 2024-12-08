using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    IEnumerator currentAttackCoroutine;

    // Patrol
    float stopMoveTimer;
    Vector3 startPosition;
    Vector3 targetPosition;


    bool isFiring = false;
    bool isJumping = false;
    bool dead = false;

    // Animation
    Animator anim;
    int shootAnim;
    int dieAnim;
    int shootEndAnim;
    int yVelAnim;
    int isWalkingAnim;

    public CompositeCollider2D foregroundCollider;

    // Jump positions
    List<Vector3> positions = new List<Vector3>();
    int currentPositionIndex = 0;

    protected override void Start()
    {
        base.Start();
        stopMoveTimer = Random.Range(minMoveCD, maxMoveCD);
        
        startPosition = transform.position;
        targetPosition = new Vector3(startPosition.x + Random.Range(-patrolDistance, patrolDistance), transform.position.y, transform.position.z);
        
        // animation stuff
        anim = GetComponent<Animator>();
        shootAnim = Animator.StringToHash("Shoot");
        dieAnim = Animator.StringToHash("Die");
        shootEndAnim = Animator.StringToHash("ShootEnd");
        yVelAnim = Animator.StringToHash("yVel");
        isWalkingAnim = Animator.StringToHash("isWalking");

        foreach(Transform child in transform)
            positions.Add(child.position);
    }

    protected override void Update()
    {
        if(dead) return;
        if(!isFiring && !isJumping && stopMoveTimer <= 0)
            Patrol();

        // if(Mathf.Abs(rb.velocity.y) < Mathf.Epsilon && Mathf.Abs(rb.velocity.x) > Mathf.Epsilon)
        // {
        //     rb.velocity = Vector3.zero;
        //     isJumping = false;
        //     stopMoveTimer = Random.Range(minMoveCD, maxMoveCD);
        // }

        stopMoveTimer -= Time.deltaTime;
        fireTimer -= Time.deltaTime;

        // Only fire after stop moving
        if (!isFiring && !isJumping && fireTimer <= 0 && stopMoveTimer >= 0)
        {
            Fire();
        }

        anim.SetFloat(yVelAnim, rb.velocity.y);
    }

    // This is the same as Enemy1. Need to change later
    protected override void Patrol()
    {
        // Don't move while firing
        // if(isFiring) return;
        // stopMoveTimer -= Time.deltaTime;
        // if(stopMoveTimer >= 0)
        //     return;

        // anim.SetBool(isWalkingAnim, true);
        // float scale = Mathf.Abs(transform.localScale.x);
        // transform.localScale = new Vector3(targetPosition.x > transform.position.x ? scale : -scale, scale, scale);

        // transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        // if(transform.position == targetPosition)
        // {
        //     anim.SetBool(isWalkingAnim, false);
        //     stopMoveTimer = Random.Range(minMoveCD, maxMoveCD);

        //     targetPosition = new Vector3(startPosition.x + Random.Range(-patrolDistance, patrolDistance), transform.position.y, transform.position.z);
        // }

        float t = 2f;
        int targetPositionIndex = Random.Range(0, positions.Count);
        while(positions.Count != 1 && targetPositionIndex == currentPositionIndex)
            targetPositionIndex = Random.Range(0, positions.Count);

        Vector3 currPosition = transform.position;
        Vector3 targetPosition = positions[targetPositionIndex];

        float dx = targetPosition.x - currPosition.x;
        float dy = targetPosition.y - currPosition.y;

        transform.localScale = new Vector3(Mathf.Sign(dx) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        Vector3 newV = new Vector3(dx / t, (dy + 0.5f * 9.81f * t * t) / t, 0);
        rb.velocity = newV;
        currentPositionIndex = targetPositionIndex;
        isJumping = true;

        StartCoroutine(StopJumping(t));
    }

    IEnumerator StopJumping(float jumpTime)
    {
        yield return new WaitForSeconds(jumpTime);
        rb.velocity = Vector3.zero;
        isJumping = false;
        stopMoveTimer = Random.Range(minMoveCD, maxMoveCD);
    }
    
    protected override void Fire()
    {
        Attack[] attacks = { SpiralAttack, DivergeAttack, UpwardAttack };
        int index = Random.Range(0, attacks.Length);
        isFiring = true;
        currentAttackCoroutine = attacks[index]();
        StartCoroutine(currentAttackCoroutine);
    }

    IEnumerator Float()
    {
        rb.velocity = new Vector3(0f, 10f, 0f);
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector3.zero;
        rb.gravityScale = 0;
    }

    IEnumerator SpiralAttack()
    {
        float prob = Random.Range(0f, 1f);
        if(prob < 0.5f)
            yield return StartCoroutine(Float());

        float spawnDistance = 1f;
        anim.SetTrigger(shootAnim);
        for(int i = 0; i < spiralFireCount; i++)
        {
            float angle = 360f / spiralFireCount * i;

            for(int j = 0; j < 4; j++)
            {
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;
                GameObject b = Instantiate(bulletPrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
                Bullet bulletScript = b.GetComponent<Bullet>();
                bulletScript.SetDirection(direction);
                angle += 90f;
            }

            yield return new WaitForSeconds(0.1f);
        }

        rb.gravityScale = 1f;
        fireTimer = fireInterval; // 重置計時器
        isFiring = false;
        stopMoveTimer = Mathf.Max(1f, stopMoveTimer);
        anim.SetTrigger(shootEndAnim);
    }

    IEnumerator DivergeAttack()
    {
        float prob = Random.Range(0f, 1f);
        if(prob < 0.5f)
            yield return StartCoroutine(Float());

        float spawnDistance = 1f;
        float initPhase = Random.Range(0f, 360f);
        for(int i = 0; i < divergeWaveCount; i++)
        {
            anim.SetTrigger(shootAnim);
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

        rb.gravityScale = 1f;
        fireTimer = fireInterval; // 重置計時器
        isFiring = false;
        stopMoveTimer = Mathf.Max(1f, stopMoveTimer);
        anim.SetTrigger(shootEndAnim);
    }

    IEnumerator UpwardAttack()
    {
        float spawnDistance = 2f;
        for(int i = 0; i < upwardWaveCount; i++)
        {
            anim.SetTrigger(shootAnim);
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

        fireTimer = fireInterval; // 重置計時器
        isFiring = false;
        anim.SetTrigger(shootEndAnim);
    }

    protected override void Die()
    {
        if(dead) // boss only die once
            return;

        dead = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        anim.SetFloat(yVelAnim, 0f);
        anim.SetTrigger(dieAnim);

        if(currentAttackCoroutine != null)
            StopCoroutine(currentAttackCoroutine);
        Bullet[] all_bullets = FindObjectsOfType<Bullet>();
        foreach(Bullet bullet in all_bullets)
            Destroy(bullet.gameObject);

        Time.timeScale = 0;

        StartCoroutine(ShowWinPanel());
    }

    IEnumerator ShowWinPanel()
    {
        yield return new WaitForSecondsRealtime(2f);

        // Time.timeScale = 0;
        GameManager.Instance.showEnd(true);
        // Transform statusUI = GameObject.Find("StatusUI")?.transform;
        // Transform LevelTimerTransform = statusUI?.Find("LevelTimer");
        // GameObject LevelTimer = LevelTimerTransform?.gameObject;
        // LevelTimer?.GetComponent<LevelTimer>().ShowCompletionTime();
        // if(WinPanel != null)
        //     WinPanel.SetActive(true);
    }
}
