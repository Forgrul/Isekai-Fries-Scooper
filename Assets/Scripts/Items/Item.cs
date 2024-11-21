using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour
{
    public float respawnTime = 10f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && GetComponent<SpriteRenderer>().enabled)
        {
            TriggerItem(other.gameObject);
            StartCoroutine(DisappearAndRespawn());
            // other.gameObject.GetComponent<PlayerController>().CollectFloatItem();
        }
    }

    IEnumerator DisappearAndRespawn()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(respawnTime);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    protected abstract void TriggerItem(GameObject player);
}