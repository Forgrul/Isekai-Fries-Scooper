using UnityEngine;

public class FloatItem : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        /*// 檢查碰撞的物件是否是 TargetObject
        if (collision.gameObject.CompareTag("player")) // 確保已設置標籤
        {
            collision.gameObject.GetComponent<PlayerController>().CollectFloatItem();
        }*/
    }
}