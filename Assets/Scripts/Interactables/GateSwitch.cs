using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSwitch : MonoBehaviour
{
    BoxCollider2D playerCollider;
    Collider2D switchCollider;
    Gate gate;

    void Awake()
    {
        playerCollider = FindObjectOfType<Player>().GetComponent<BoxCollider2D>();
        switchCollider = GetComponent<Collider2D>();
        gate = transform.parent.GetComponentInChildren<Gate>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            gate.OpenGate();
    }
}
