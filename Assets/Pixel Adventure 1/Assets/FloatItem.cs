using UnityEngine;
using System.Collections;

public class FloatItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // 檢查碰撞的物件是否是 TargetObject
        if (collision.gameObject.CompareTag("Player")) // 確保已設置標籤
        {
            GameManager.Instance.ChangeFloatStatus(true);
            StartCoroutine(BackToGround());
            // collision.gameObject.GetComponent<PlayerController>().CollectFloatItem();
        }
    }

    IEnumerator BackToGround()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Print the message to the console
        GameManager.Instance.ChangeFloatStatus(false);
    }
}