using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;       // 移動速度
    public float jumpForce = 10f;      // 跳躍力
    public LayerMask groundLayer;      // 定義地面圖層
    public float CharacterSize = 0.5f; // 角色大小
    public float GScale = 3;           // 重力大小  
    
    private Rigidbody2D rb;
    private bool isGrounded;           // 判斷是否在地面上
    bool flipLocked = false;

    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashTime;
    bool canDash = true;               // can't dash again when dashing, can't dash twice in the air
    bool isDashing = false;            // player can't move while dashing

    [Header("FastLanding Settings")]
    public float fastLandingSpeed = 10f;

    [Header("Deflect Settings")]
    public float deflectTime = 0.25f;
    public float deflectCD = 0.5f;
    public DeflectArea deflectArea;
    bool isDeflecting = false;
    bool canDeflect = true;
    float timer;

    // Animation
    Animator anim;
    // Animation conditions
    int deflectAnim;
    int isWalkingAnim;
    int vertVAnim;
    int dashAnim;

    private bool isFloating
    {
        get {return GameManager.Instance.GetFloatStatus();}
    }   // 判斷是否在能浮空

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        deflectAnim = Animator.StringToHash("deflect");
        isWalkingAnim = Animator.StringToHash("isWalking");
        vertVAnim = Animator.StringToHash("vertV");
        dashAnim = Animator.StringToHash("dash");
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
            if(isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
            {
                Jump();
            }
            if(canDash && Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
            }
            if(!isGrounded && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
            {
                FastLanding();
            }

            anim.SetFloat(vertVAnim, rb.velocity.y);
        }

        if(canDeflect && !isDashing && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Deflect());
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

        anim.SetBool(isWalkingAnim, moveInput != 0);

        // Flip the character based on horizontal input
        if(flipLocked) return;
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
        anim.SetTrigger(dashAnim);

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
        rb.velocity = new Vector3(rb.velocity.x, fastLandingSpeed * -1, 0f);
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

    IEnumerator Deflect()
    {
        float angle = deflectArea.StartDeflect();
        isDeflecting = true;
        canDeflect = false;
        anim.SetTrigger(deflectAnim);
        flipLocked = true;
        if(angle < 90f && angle > -90f) 
            transform.localScale = new Vector3(Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face right
        else
            transform.localScale = new Vector3(-Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face left

        yield return new WaitForSeconds(deflectTime);

        deflectArea.StopDeflect();
        isDeflecting= false;
        flipLocked = false;

        yield return new WaitForSeconds(deflectCD);

        canDeflect = true;
    }
}
