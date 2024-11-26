using UnityEngine;

public class EnemyBullet : Bullet
{
    public Color deflectedColor;
    bool isTouchingEnemy = false;
    Enemy touchEnemy;
    bool deflected = false;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if(!deflected && other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(player.IsInvincible())
                return;
            player.GetHit();
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            isTouchingEnemy = true;
            touchEnemy = other.gameObject.GetComponent<Enemy>();
            if(deflected)
            {
                touchEnemy.GetHit();
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
            isTouchingEnemy = false;
    }

    public void Deflect(Vector3 direction)
    {
        PlayHitbackSound();

        rb.velocity = direction * vel;  
        deflected = true;
        GetComponent<SpriteRenderer>().color = deflectedColor;
        if(isTouchingEnemy)
        {
            touchEnemy.GetHit();
            Destroy(gameObject);
        }
    }

    public AudioClip hitbackSound;

    // private AudioSource audioSource;
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

    public void PlayHitbackSound()
    {
        PlaySound(hitbackSound);
    }
}
