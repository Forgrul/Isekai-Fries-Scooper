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
            // other.gameObject.GetComponent<Player>().GetHit();
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
