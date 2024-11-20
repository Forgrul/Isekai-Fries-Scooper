using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    // LevelLoader is used to initialize the game state upon scene load

    void Start()
    {
        GameManager.Instance.ChangeFloatStatus(false);
    }
}
