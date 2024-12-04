using System.Collections;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    public Collider2D playerCollider;
    Rigidbody2D playerRb;
    bool canRecover = false;
    CompositeCollider2D platformCollider;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }

        if(canRecover && playerRb.velocity.y >= 0f)
        {
            canRecover = false;
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            if(canRecover)
            {
                canRecover = false;
                Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
            }
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        platformCollider = currentOneWayPlatform.GetComponent<CompositeCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.05f);
        canRecover = true;
        // Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}