using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public float openHeight = 2f;
    public float openSpeed = 2f;

    Vector3 openPosition;
    Vector3 closePosition;
    // default is closed
    bool opened = false;
    // Start is called before the first frame update
    void Start()
    {
        closePosition = transform.position;
        openPosition = closePosition + new Vector3(0, openHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(opened)
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, closePosition, openSpeed * Time.deltaTime);
    }
    
    public void ToggleState()
    {
        opened = !opened;
    }
}
