using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;       // 移動速度
    public float jumpForce = 10f;      // 跳躍力
    public LayerMask groundLayer;      // 定義地面圖層
    public float CharacterSize = 0.5f; // 角色大小
    public float GScale = 3;           // 重力大小  

    // public Transform groundCheck;
    // public float checkRadius = 0.1f;   // 檢測範圍

    // public Collider2D groundDetector;
    
    private Rigidbody2D rb;
    private bool isGrounded;           // 判斷是否在地面上

    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashTime;
    bool canDash = true;               // can't dash again when dashing, can't dash twice in the air
    bool isDashing = false;            // player can't move while dashing

    [Header("FastLanding Settings")]
    public float fastLandingSpeed = 10f;


    private bool isFloating
    {
        get {return GameManager.Instance.GetFloatStatus();}
    }   // 判斷是否在能浮空



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isFloating)
        {
            FlyMove();
        }
        else
        {
            if(!isDashing)
            {
                GroundMove();
            }
            if(isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if(canDash && Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
            }
            if(!isGrounded && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && Input.GetKeyDown(KeyCode.Space))
            {
                FastLanding();
            }
        }
    }

    void FlyMove()
    {
        rb.gravityScale = 0;

        // Control vertical movement
        float moveInputVertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(rb.velocity.x, moveInputVertical * moveSpeed);

        // Add horizontal movement while floating
        float moveInputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInputHorizontal * moveSpeed, rb.velocity.y);
        
        // Flip the character based on horizontal input
        if (moveInputHorizontal > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face right
        }
        else if (moveInputHorizontal < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face left
        }
    }

    void GroundMove()
    {
        rb.gravityScale = GScale;
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip the character based on horizontal input
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face right
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face left
        }   
    }

    void Jump()
    {
        isDashing = false;
        canDash = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Set jump force
    }

    IEnumerator Dash()
    {
        canDash = false;
        rb.velocity = new Vector3(transform.localScale.x * dashSpeed, 0f, 0f);
        rb.gravityScale = 0;
        isDashing = true;

        yield return new WaitForSeconds(dashTime);

        if(!isDashing)
            yield break;
        if(isGrounded)
            canDash = true;
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.gravityScale = GScale;
        isDashing = false;
    }

    void FastLanding()
    {
        isDashing = false;
        rb.velocity = new Vector3(0f, fastLandingSpeed * -1, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0) // 使用 Layer 檢查是否為地面
        {
            isGrounded = true; // 設定為在地面上
            if(!isDashing)
                canDash = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0) // 使用 Layer 檢查是否為地面
        {
            isGrounded = false; // 設定為離開地面
        }
    }
}
