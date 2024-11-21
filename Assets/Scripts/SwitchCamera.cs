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
            followCam.enabled = false;
            fixedCam.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            followCam.enabled = true;
            fixedCam.enabled = false;
        }
    }
}
