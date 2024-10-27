using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;               // 移動速度
    public float jumpForce = 10f;             // 跳躍力度
    public Transform groundCheck;              // 地面檢查的 Transform
    public float groundCheckRadius = 0.1f;     // 地面檢查的半徑
    public LayerMask groundLayer;              // 地面層
    public float CharacterSize = 0.3f;         // 角色大小

    public float GScale = 3;

    private bool isGrounded;                   // 角色是否在地面上
    private bool noGravityMode = false;        // 是否無重力模式
    private Rigidbody2D rb;                    // 角色的 Rigidbody2D

    private bool canJump = true;                // 是否可以跳躍

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // 獲取 Rigidbody2D 組件
    }

    void Update()
    {
        // 檢查是否無重力模式
        if (noGravityMode)
        {
            // 自由上下左右移動
            rb.gravityScale = 0;
            float moveInputX = Input.GetAxis("Horizontal");
            float moveInputY = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(moveInputX * moveSpeed, moveInputY * moveSpeed);
        }
        else
        {
            
            rb.gravityScale = GScale;

            // 使用 Circle 檢查是否在地面上
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            
            // 獲取左右移動輸入
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            // 跳躍邏輯
            if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // 設置跳躍力度
                isGrounded = false; // 跳躍後設為不在地面
                Debug.Log("已跳躍" + isGrounded);
            }

            // 確保角色不會穿過地面
            if (transform.position.y <= groundCheck.position.y)
            {
                transform.position = new Vector3(transform.position.x, groundCheck.position.y, transform.position.z);
                Debug.Log("角色已重置到地面");
            }
        }

        // 調整角色面朝方向
        if (Input.GetAxis("Horizontal") > 0)
            transform.localScale = new Vector3(CharacterSize, CharacterSize, CharacterSize);  // 面朝右
        else if (Input.GetAxis("Horizontal") < 0)
            transform.localScale = new Vector3((-1 * CharacterSize), CharacterSize, CharacterSize); // 面朝左
    }

    private void FixedUpdate()
    {
        if (!noGravityMode)
        {
            // 使用 Circle 檢查是否在地面上
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            Debug.Log("isGrounded: " + isGrounded);
        }
    }

    // 偵測碰撞道具 X 的觸發方法
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FloatItem")) // 檢查是否是道具 X
        {
            Debug.Log("FloatItem picked up!");
            noGravityMode = true; // 啟用無重力模式
            rb.gravityScale = 0;  // 關閉重力
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FloatItem")) // 檢查是否結束道具 X 的效果
        {
            noGravityMode = false; // 恢復重力模式
            rb.gravityScale = GScale;   // 開啟重力
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
