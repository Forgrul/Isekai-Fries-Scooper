using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;         // 移動速度

    [Header("Health Settings")]
    public int health = 3;

    [Header("Fire Settings")]
    public GameObject bulletPrefab;
    public float fireInterval = 3f;  // 發射間隔
    public float hardCoreFireIntervalFactor = 0.6f;
    protected float fireTimer;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(GameManager.Instance.isHardcore)
            SetUpHardcore();
        fireTimer = 1f; 
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

    // 翻轉敵人朝向
    protected void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // 巡邏方法
    protected abstract void Patrol();

    // 發射方法
    protected abstract void Fire();

    public void GetHit()
    {
        health--;
        PlayHurtEnemySound();

        if (health <= 0)
        {
            Die();
        }
    }

    void SetUpHardcore()
    {
        fireInterval *= hardCoreFireIntervalFactor;
    }

    //sound effects
    public AudioClip hurt_enemySound;
    public void PlaySound(AudioClip clip)
    {
        // 動態創建 AudioSource
        GameObject tempAudio = new GameObject("TempAudio");
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;

        // 設置音效屬性
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; // 2D 音效
        audioSource.loop = false;

        // 播放音效
        audioSource.Play();

        // 銷毀音效物件
        Destroy(tempAudio, clip.length);
    }

    public void PlayHurtEnemySound()
    {
        PlaySound(hurt_enemySound);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
