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
        rb.velocity = direction * vel;  
        deflected = true;
        GetComponent<SpriteRenderer>().color = deflectedColor;
        if(isTouchingEnemy)
        {
            touchEnemy.GetHit();
            Destroy(gameObject);
        }
    }
}
