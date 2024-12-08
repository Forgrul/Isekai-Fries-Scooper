using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Gate : MonoBehaviour
{
    public float openHeight = 2f;
    public float openSpeed = 2f;

    Vector3 openPosition;
    Vector3 closePosition;
    // default is closed
    bool opened = false;
    bool canMove = false;

    public float switchCameraTime = 1f;
    CinemachineBrain brain;
    CinemachineVirtualCamera gateCamera;
    
    void Start()
    {
        closePosition = transform.position;
        openPosition = closePosition + new Vector3(0, openHeight, 0);
        brain = Camera.main.GetComponent<CinemachineBrain>();
        gateCamera = GetComponent<CinemachineVirtualCamera>();
        gateCamera.enabled = false;
    }

    
    void Update()
    {
        if(canMove)
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);
    }
    
    public void OpenGate()
    {
        if(opened) return;
        opened = true;
        StartCoroutine(SwitchCameraAndOpenGate());
    }

    IEnumerator SwitchCameraAndOpenGate()
    {
        // another 2 second so player is not immediately damaged after camera restore
        Player playerScript = FindObjectOfType<Player>().GetComponent<Player>();
        StartCoroutine(playerScript.SetInvincible(switchCameraTime * 2 + 2f));

        var followCamera = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        gateCamera.enabled = true;
        followCamera.enabled = false;
        yield return new WaitForSeconds(switchCameraTime);
        canMove = true;
        yield return new WaitForSeconds(switchCameraTime);
        followCamera.enabled = true;
        gateCamera.enabled = false;
    }
}
