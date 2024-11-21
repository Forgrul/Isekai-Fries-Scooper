using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float vel = 10; // The length of the vector of initial V.
    public int bounceCountMax = 3; // The bullet will disappear after exceeding the number of bounces

    protected int bounceCount;
    protected Rigidbody2D rb;

    protected void Awake()
    {
        bounceCount = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    // Updates rotation and destroys after enough bounces
    protected void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if(bounceCount >= bounceCountMax)
            Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction)
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
        if(other.gameObject.CompareTag("Platform"))
        {
            bounceCount++;

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
