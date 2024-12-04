using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchCamera : MonoBehaviour
{
    public CinemachineVirtualCamera followCam;
    public CinemachineVirtualCamera fixedCam;
    
    void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.gameObject.CompareTag("Player"))
        {
            SwitchToFixedCamera();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SwitchToFollowCamera();
        }
    }

    void SwitchToFollowCamera()
    {
        followCam.enabled = true;
        if(fixedCam != null)
            fixedCam.enabled = false;
    }

    void SwitchToFixedCamera()
    {
        followCam.enabled = false;
        if(fixedCam != null)
            fixedCam.enabled = true;
    }
}
