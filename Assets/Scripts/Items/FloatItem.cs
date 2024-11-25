using UnityEngine;
using System.Collections;

public class FloatItem : Item
{
    public float duration = 5f;

    private GameObject FloatingTimer;

    void Start()
    {
        Transform statusUI =  GameObject.Find("StatusUI").transform;
        Transform floatingTimerTransform = statusUI.Find("FloatingTimer");
        FloatingTimer = floatingTimerTransform.gameObject;
    }

    protected override void TriggerItem(GameObject player)
    {
        GameManager.Instance.ChangeFloatStatus(true);
        FloatingTimer.SetActive(true);
        FloatingTimer.GetComponent<FloatingTimer>().StartTimer(duration);
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