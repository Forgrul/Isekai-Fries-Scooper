using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float initV = 10; // The length of the vector of initial V.
    public float initTheta = 0; // The degree of the vector of initial V.
    public int bounceCountMax = 3; // The bullet will disappear after exceeding the number of bounces
    public int damage = 20;

    private Vector2 nowV;
    private int bounceCount = 0;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            nowV = new Vector2(initV * Mathf.Cos(initTheta * Mathf.Deg2Rad), initV * Mathf.Sin(initTheta * Mathf.Deg2Rad));
            rb.velocity = nowV;
        }
    }

    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if(bounceCount >= bounceCountMax)
            Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // Damage
            collision.gameObject.GetComponent<Player>().GetHit(damage);
            Destroy(gameObject);
        }
        else if(!collision.gameObject.CompareTag("Enemy"))
        {
            bounceCount++;
            // Vector2 normal = collision.contacts[0].normal;
            // Vector2 refV = Vector2.Reflect(nowV, normal);
            // rb.velocity = refV;
            // nowV = refV;

            RaycastHit2D hit;
            LayerMask mask = LayerMask.GetMask("Ground");
            hit = Physics2D.Raycast(transform.position, rb.velocity.normalized, Mathf.Infinity, mask);

            if (hit.collider != null)
            {
                Vector2 refV = Vector3.Reflect(rb.velocity, hit.normal);
                rb.velocity = refV;
            }
        }   
    }
}
