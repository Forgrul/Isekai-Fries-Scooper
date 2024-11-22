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
    }
}
