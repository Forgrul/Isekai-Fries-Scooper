using UnityEngine;

public class EnemyBullet : Bullet
{
    public Color deflectedColor;
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
        else if(deflected && other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().GetHit();
            Destroy(gameObject);
        }
    }

    public void Deflect(Vector3 direction)
    {
        rb.velocity = direction * vel;  
        deflected = true;
        GetComponent<SpriteRenderer>().color = deflectedColor;
        PlayHitbackSoundSound();

    }

    public AudioClip hitbackSoundSound;

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

    public void PlayHitbackSoundSound()
    {
        PlaySound(hitbackSoundSound);
    }
}
