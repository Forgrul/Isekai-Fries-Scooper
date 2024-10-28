using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;       // 移動速度
    public float jumpForce = 10f;      // 跳躍力
    public LayerMask groundLayer;      // 定義地面圖層
    public float CharacterSize = 0.5f; // 角色大小
    public float GScale = 3;           // 重力大小  

    public Transform groundCheck;
    public float checkRadius = 0.1f;  // 檢測範圍
    
    
    private Rigidbody2D rb;
    private bool isGrounded;           // 判斷是否在地面上
    private bool isFloating = false;           // 判斷是否在能浮空


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        
        if (isFloating)
        {
            rb.gravityScale = 0;

            // 控制上下移動
            float moveInputVertical = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, moveInputVertical * moveSpeed);
            
            // 可以在這裡添加左右移動，若想要在浮空時也能左右移動
            float moveInputHorizontal = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInputHorizontal * moveSpeed, rb.velocity.y);
        }
        else{
            rb.gravityScale = GScale;
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);


            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = new Vector3(CharacterSize, CharacterSize, CharacterSize);  // 面朝右
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-CharacterSize, CharacterSize, CharacterSize); // 面朝左
            }

            // 跳躍
            if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // 設置跳躍力度
            }
        }
        
    }


    // 當角色與地面（Tilemap）接觸時觸發
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0) // 使用 Layer 檢查是否為地面
        {
            isGrounded = true; // 設定為在地面上
        }
    }

    // 當角色離開地面（Tilemap）時觸發
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0) // 使用 Layer 檢查是否為地面
        {
            isGrounded = false; // 設定為離開地面
        }
    }
    
    public void CollectFloatItem()
    {
        isFloating = !isFloating; // 切換浮空狀態
    }
    
}
