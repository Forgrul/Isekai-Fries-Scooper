using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public LayerMask groundLayer;      // 定義地面圖層
    public float CharacterSize = 0.5f; // 角色大小
    public float GScale = 3;           // 重力大小  
    
    private Rigidbody2D rb;
    private bool isGrounded;           // 判斷是否在地面上
    bool flipLocked = false;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;       // 移動速度
    public float flySpeed = 9f;        // 飛行時的移動速度
    public float jumpForce = 10f;      // 跳躍力

    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashTime;
    bool canDash = true;               // can't dash again when dashing, can't dash twice in the air
    bool isDashing = false;            // player can't move while dashing

    [Header("FastLanding Settings")]
    public float fastLandingSpeed = 10f;
    bool isLanding = false;

    [Header("Deflect Settings")]
    public float deflectTime = 0.25f;
    public float deflectCD = 0.5f;
    public DeflectArea deflectArea;
    bool canDeflect = true;            // deflect has CD

    [Header("Shoot Settings")]
    public float shootTime = 0.2f;
    public float shootCD = 0.2f;
    public GameObject bulletPrefab;
    public float bulletInitDistance = 0.5f;
    int ammo = 0;
    bool canShoot = true;

    [Header("Keys")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode deflectKey = KeyCode.Mouse1;
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode[] fastLandingKeys = {KeyCode.S, KeyCode.DownArrow};
    public KeyCode[] jumpKeys = {KeyCode.Space, KeyCode.W, KeyCode.UpArrow};

    // Animation
    Animator anim;
    // Animation conditions
    int deflectAnim;
    int isWalkingAnim;
    int vertVAnim;
    int dashAnim;
    int shootAnim;
    int shootAngleAnim;
    int flyXVelAnim;
    int flyTriggerAnim;
    int endFlyTrigger;

    // Trail
    TrailRenderer trail;

    // Particle System
    ParticleSystem myParticleSystem;
    Vector3 psIdlePosition = new Vector3(-0.5f, -0.5f, 0);
    Vector3 psMovingPosition = new Vector3(-0.85f, -0.4f, 0);
    float psIdleZRotation = -105f;
    float psMovingZRotation = -130f;

    float flyTimer = 0f;

    // Sound Effects

    public AudioClip dashSound;
    public AudioClip gun_pickSound; // todo

    public AudioClip gun_shotSound; // todo

    public AudioClip item_pickSound;

    public AudioClip walkingSound;
    public AudioClip deflectSound;

    // private AudioSource audioSource;
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        // 動態創建 AudioSource
        GameObject tempAudio = new GameObject("TempAudio");
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;

        // 設置音效屬性
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; // 2D 音效
        audioSource.loop = false;
        audioSource.volume = volume;

        // 播放音效
        audioSource.Play();

        // 銷毀音效物件
        Destroy(tempAudio, clip.length);
    }

    public void PlayDashSound()
    {
        PlaySound(dashSound, 0.2f);
    }
    public void PlayGunPickSound()
    {
        PlaySound(gun_pickSound);
    }
    public void PlayGunShotSound()
    {
        PlaySound(gun_shotSound);
    }

    
    public void PlayItemPickSound()
    {
        PlaySound(item_pickSound);
    }
    public void PlayWalkingSound()
    {
        PlaySound(walkingSound, 1000f);
    }
    public void PlayDeflectingSound()
    {
        PlaySound(deflectSound);
    }

    private bool canPlayWalkSound = true;
    public float walkSoundCooldown = 1f;

    IEnumerator WalkSoundCooldownCoroutine()
    {
        // Debug.LogWarning("walking sound cooling down...");
        canPlayWalkSound = false;
        yield return new WaitForSeconds(walkSoundCooldown);
        canPlayWalkSound = true;
    }

    // ----------------------------------------------------------------

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        myParticleSystem = GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
        deflectAnim = Animator.StringToHash("deflect");
        isWalkingAnim = Animator.StringToHash("isWalking");
        vertVAnim = Animator.StringToHash("vertV");
        dashAnim = Animator.StringToHash("dash");
        shootAnim = Animator.StringToHash("shoot");
        shootAngleAnim = Animator.StringToHash("shootAngle");
        flyXVelAnim = Animator.StringToHash("flyXVel");
        flyTriggerAnim = Animator.StringToHash("flyTrigger");
        endFlyTrigger = Animator.StringToHash("endFlyTrigger");
    }


    void Update()
    {
        if (flyTimer > 0)
        {
            flyTimer -= Time.deltaTime;
            FlyMove();
            if(flyTimer <= 0)
            {
                myParticleSystem.Stop();
                anim.SetTrigger(endFlyTrigger);
            }
        }
        else
        {
            if(!isDashing)
            {
                GroundMove();
            }
            if(isGrounded && jumpKeys.Any(key => Input.GetKeyDown(key)))
            {
                Jump();
            }
            if(canDash && Input.GetKeyDown(dashKey))
            {
                StartCoroutine(Dash());
            }
            if(!isGrounded && fastLandingKeys.Any(key => Input.GetKeyDown(key)))
            {
                FastLanding();
            }

            anim.SetFloat(vertVAnim, rb.velocity.y);
        }

        if(canDeflect && Input.GetKeyDown(deflectKey))
        {
            StartCoroutine(Deflect());
        }

        if(ammo > 0 && canShoot && !isDashing && Input.GetKey(shootKey))
        {
            StartCoroutine(Shoot());
        }

    }

    void FlyMove()
    {
        rb.gravityScale = 0;

        // Control vertical movement
        float moveInputVertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(rb.velocity.x, moveInputVertical * flySpeed);

        // Add horizontal movement while floating
        float moveInputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInputHorizontal * flySpeed, rb.velocity.y);
        
        // Flip the character based on horizontal input
        if (moveInputHorizontal > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face right
        }
        else if (moveInputHorizontal < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face left
        }

        float absXVel = Mathf.Abs(rb.velocity.x);
        anim.SetFloat(flyXVelAnim, absXVel);

        ParticleSystem.ShapeModule editableShape = myParticleSystem.shape;
        if(absXVel > Mathf.Epsilon)
        {
            editableShape.position = psMovingPosition;
            editableShape.rotation = new Vector3(0, 0, psMovingZRotation);
        }
        else
        {
            editableShape.position = psIdlePosition;
            editableShape.rotation = new Vector3(0, 0, psIdleZRotation);
        }
    }

    public void Fly(float duration)
    {
        anim.SetTrigger(flyTriggerAnim);
        myParticleSystem.Play();
        flyTimer = duration;
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
            if(canPlayWalkSound){
                PlayWalkingSound();
                // Debug.LogWarning("playing walk sound");
                StartCoroutine(WalkSoundCooldownCoroutine());
            }
            
            transform.localScale = new Vector3(Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face right
        }
        else if (moveInput < 0)
        {
            if(canPlayWalkSound){
                PlayWalkingSound();
                // Debug.LogWarning("playing walk sound");
                StartCoroutine(WalkSoundCooldownCoroutine());
            }
            
            transform.localScale = new Vector3(-Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face left
        }
    }

    void Jump()
    {
        isDashing = false;
        trail.emitting = false;
        canDash = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Set jump force
    }

    IEnumerator Dash()
    {
        PlayDashSound();

        canDash = false;
        if(Mathf.Abs(rb.velocity.x) > Mathf.Epsilon)
            rb.velocity = new Vector3(Mathf.Sign(rb.velocity.x) * dashSpeed, 0f, 0f);
        else
            rb.velocity = new Vector3(transform.localScale.x * dashSpeed, 0f, 0f);
        rb.gravityScale = 0;
        isDashing = true;
        anim.SetTrigger(dashAnim);
        trail.emitting = true;

        yield return new WaitForSeconds(dashTime);

        if(!isDashing)
            yield break;
        if(isGrounded)
            canDash = true;
        
        trail.emitting = false;
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.gravityScale = GScale;
        isDashing = false;
    }

    void FastLanding()
    {
        isDashing = false;
        isLanding = true;
        rb.velocity = new Vector3(rb.velocity.x, fastLandingSpeed * -1, 0f);
        trail.emitting = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0) // 使用 Layer 檢查是否為地面
        {
            isGrounded = true; // 設定為在地面上
            if(!isDashing)
                canDash = true;
            if(isLanding)
            {
                trail.emitting = false;
                isLanding = false;
            }
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
        PlayDeflectingSound();
        float angle = deflectArea.StartDeflect();
        canDeflect = false;
        anim.SetTrigger(deflectAnim);
        
        flipLocked = true;
        if(angle < 90f && angle > -90f) 
            transform.localScale = new Vector3(Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face right
        else
            transform.localScale = new Vector3(-Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face left

        yield return new WaitForSeconds(deflectTime);

        anim.ResetTrigger(deflectAnim); // to prevent it register the deflect animation when flying and play the animation after flying mode end
        deflectArea.StopDeflect();
        flipLocked = false;

        yield return new WaitForSeconds(deflectCD);

        canDeflect = true;
    }

    IEnumerator Shoot()
    {
        // audioSource.PlayOneShot(gun_shot);
        PlayGunShotSound();
        canShoot = false;
        anim.SetTrigger(shootAnim);
        ammo--;

        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction2d = (mousePosWorld - transform.position);
        Vector3 direction = direction2d.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(angle < 90f && angle > -90f)
            transform.localScale = new Vector3(Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face right
        else
            transform.localScale = new Vector3(-Mathf.Abs(CharacterSize), CharacterSize, CharacterSize); // Face left
        flipLocked = true;

        if(angle > 90f && angle < 180f)
            angle = 180f - angle;
        else if(angle < -90f && angle > -180f)
            angle = -180f - angle;

        anim.SetFloat(shootAngleAnim, angle);

        GameObject bullet = Instantiate(bulletPrefab, transform.position + direction * bulletInitDistance, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);

        yield return new WaitForSeconds(shootCD);

        canShoot = true;
        flipLocked = false;
    }

    public void GetShootItem(int _ammo)
    {
        ammo += _ammo;
        PlayGunPickSound();
    }

    public int GetAmmo()
    {
        return ammo;
    }
}
