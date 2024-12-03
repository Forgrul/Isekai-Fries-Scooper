using UnityEngine;
using System.Collections;

public abstract class Bullet : MonoBehaviour
{
    public float vel = 10; // The length of the vector of initial V.
    public int bounceCountMax = 3; // The bullet will disappear after exceeding the number of bounces
    public Sprite scatteredImage;

    protected int bounceCount;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        bounceCount = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {

    }

    // Updates rotation and destroys after enough bounces
    protected virtual void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if(bounceCount >= bounceCountMax)
            Destroy(gameObject);
    }

    public virtual void SetDirection(Vector3 direction)
    {
        rb.velocity = direction * vel;
    }

    public void SetDirectionAngle(float angle)
    {
        // rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * vel;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("OneWayPlatform"))
        {
            bounceCount++;
            if(bounceCountMax != 1 && bounceCount == bounceCountMax - 1)
            {
                transform.localScale = (0.5f + 0.5f * (float)(bounceCountMax - bounceCount) / (float)bounceCountMax) * transform.localScale;
                if(scatteredImage != null)
                    GetComponent<SpriteRenderer>().sprite = scatteredImage;
            }

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
