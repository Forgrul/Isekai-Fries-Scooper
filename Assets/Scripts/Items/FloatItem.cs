using UnityEngine;
using System.Collections;

public class FloatItem : Item
{
    public float duration = 5f;

    protected override void TriggerItem(GameObject player)
    {
        GameManager.Instance.ChangeFloatStatus(true);
        StartCoroutine(BackToGround());
    }

    IEnumerator BackToGround()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(duration);

        // Print the message to the console
        GameManager.Instance.ChangeFloatStatus(false);
    }
}