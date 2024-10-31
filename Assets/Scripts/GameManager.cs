using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    bool isFloating = false; 

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameManager persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void ChangeFloatStatus(bool newStatus)
    {
        isFloating = newStatus;
    }

    public bool GetFloatStatus()
    {
        return isFloating;
    }
}
