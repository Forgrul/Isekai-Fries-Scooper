using UnityEngine;
using System.Collections;

public class FloatItem : MonoBehaviour
{
    public float respawnTime = 10f;
    public float duration = 5f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 檢查碰撞的物件是否是 TargetObject
        if (collision.gameObject.CompareTag("Player") && GetComponent<SpriteRenderer>().enabled) // 確保已設置標籤
        {
            GameManager.Instance.ChangeFloatStatus(true);
            StartCoroutine(BackToGround());
            StartCoroutine(DisappearAndRespawn());
            // collision.gameObject.GetComponent<PlayerController>().CollectFloatItem();
        }
    }

    IEnumerator BackToGround()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(duration);

        // Print the message to the console
        GameManager.Instance.ChangeFloatStatus(false);
    }

    IEnumerator DisappearAndRespawn()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(respawnTime);
        GetComponent<SpriteRenderer>().enabled = true;
    }
}