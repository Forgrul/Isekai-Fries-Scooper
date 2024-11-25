using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour
{
    public bool canRespawn = true;
    public float respawnTime = 10f;

    public delegate void ItemTaken();
    public event ItemTaken OnItemTaken;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && GetComponent<SpriteRenderer>().enabled)
        {
            TriggerItem(other.gameObject);
            OnItemTaken?.Invoke();
            StartCoroutine(DisappearAndRespawn());
        }
    }

    IEnumerator DisappearAndRespawn()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        if(canRespawn)
        {
            yield return new WaitForSeconds(respawnTime);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    protected abstract void TriggerItem(GameObject player);
}