using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;         // 移動速度
    public float patrolDistance = 2f;    // 巡邏範圍
    public float floatSpeed = 2f;        // 浮空時的移動速度
    public float floatInterval = 1f;     // 浮空狀態下每段的移動時間

    private Vector2 startPosition;       // 初始位置
    private bool movingRight = true;     // 是否向右移動
    private bool isFloating              // 是否處於浮空狀態
    {
        get { return GameManager.Instance.GetFloatStatus(); }
    }     
    private float floatTimer = 0f;       // 計時器
    private int floatDirection = 0;      // 浮空方向 (0: 向右, 1: 向上, 2: 向左, 3: 向下)

    public GameObject bullet;
    public float fireInterval = 3f;  // 發射間隔
    public float bulletSpeed = 3f;  // 子彈速度

    private float fireTimer;
    private Rigidbody2D rb;
    private float tmp = 0.2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        fireTimer = fireInterval; 
    }

    private void Update()
    {
        if (isFloating)
        {
            if(tmp > 0){
                rb.velocity = Vector2.up * floatSpeed;
            }
            else {
                MoveInSquare();
            }
            tmp -= Time.deltaTime;
        }
        else
        {
            Patrol();
            tmp = 0.2f;
        }

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0)
        {
            FireHexagonalBullets();
            fireTimer = fireInterval; // 重置計時器
        }
    }

    // 巡邏方法
    private void Patrol()
    {
        rb.gravityScale = 1f; // 重力開啟
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

    // 浮空狀態的移動方法
    private void MoveInSquare()
    {
        rb.gravityScale = 0f; // 無視重力
        floatTimer += Time.deltaTime;

        // 檢查是否達到方向切換的時間
        if (floatTimer >= floatInterval)
        {
            floatDirection = (floatDirection + 1) % 4; // 切換到下一個方向
            floatTimer = 0f;
        }

        // 根據方向移動
        Vector2 moveDir = Vector2.zero;
        switch (floatDirection)
        {
            case 0: moveDir = Vector2.right; break;
            case 1: moveDir = Vector2.up; break;
            case 2: moveDir = Vector2.left; break;
            case 3: moveDir = Vector2.down; break;
        }

        rb.velocity = moveDir * floatSpeed;
    }

    // 翻轉敵人朝向
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // 切換浮空狀態方法
    public void ToggleFloating()
    {
        // If the enemy is switching to patrolling, reset the float timer and direction
        if (!isFloating)
        {
            floatTimer = 0f;          // Reset float timer
            floatDirection = 0;       // Reset float direction
            rb.velocity = Vector2.zero; // Stop any ongoing movement
        }
    }

    void FireHexagonalBullets()
    {
        int bulletCount = 6; // 六邊形的 6 顆子彈
        float angleStep = 360f / bulletCount; // 每顆子彈的角度間隔
        float spawnDistance = 0.7f; // 生成位置偏移距離

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            
            Vector2 spawnPosition = (Vector2)transform.position + direction * spawnDistance;
            GameObject b = Instantiate(bullet, spawnPosition, Quaternion.Euler(0, 0, angle));
            Bullet bulletScript = b.GetComponent<Bullet>();

            bulletScript.initV = bulletSpeed;
            bulletScript.initTheta = angle;
        }
    }
}
