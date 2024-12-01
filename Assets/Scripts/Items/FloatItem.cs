using UnityEngine;
using System.Collections;

public class FloatItem : Item
{
    public float duration = 5f;

    private GameObject FloatingTimer;

    void Start()
    {
        Transform statusUI = GameObject.Find("StatusUI").transform;
        Transform floatingTimerTransform = statusUI.Find("FloatingTimer");
        FloatingTimer = floatingTimerTransform.gameObject;
    }

    protected override void TriggerItem(GameObject player)
    {
        PlayerController playerControllerScript = player.GetComponent<PlayerController>();
        playerControllerScript.Fly(duration);

        FloatingTimer.SetActive(true);
        FloatingTimer.GetComponent<FloatingTimer>().StartTimer(duration);
    }
}