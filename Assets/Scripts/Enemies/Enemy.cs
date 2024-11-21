using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;         // 移動速度
    public float patrolDistance = 2f;    // 巡邏範圍
    private bool movingRight = true;     // 是否向右移動
    private Vector3 startPosition;

    [Header("Health Settings")]
    public int health = 3;

    [Header("Fire Settings")]
    public GameObject bulletPrefab;
    public float fireInterval = 3f;  // 發射間隔
    private float fireTimer;

    protected Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        fireTimer = fireInterval; 
    }

    protected virtual void Update()
    {
        Patrol();

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0)
        {
            Fire();
            fireTimer = fireInterval; // 重置計時器
        }
    }

    // 巡邏方法
    protected virtual void Patrol()
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

    // 翻轉敵人朝向
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected abstract void Fire();

    public void GetHit()
    {
        health--;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
