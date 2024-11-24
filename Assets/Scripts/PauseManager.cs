using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public Canvas PauseCanvas;

    public bool ispaused = false;

    void Start()
    {
        Time.timeScale = 1;
        PauseCanvas.gameObject.SetActive(false);
        ispaused = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(ispaused)
            {
                Time.timeScale = 1;
                PauseCanvas.gameObject.SetActive(false);
                ispaused = false;
            }
            else
            {
                Time.timeScale = 0;
                PauseCanvas.gameObject.SetActive(true);
                ispaused = true;
            }
        }
    }

}
