using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public TMP_Text timerText; 
    public TMP_Text completionTimeText; // 顯示通關時間的 UI
    private float elapsedTime = 0f;
    private bool isTiming = false; 

    void Start()
    {
        StartTimer();
        timerText = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime; 
            UpdateTimerDisplay();
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f; 
        isTiming = true;  
    }

    public void StopTimer()
    {
        isTiming = false; 
        Debug.Log("Final Time: " + FormatTime(elapsedTime));
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = FormatTime(elapsedTime); 
        }
    }

    public void ShowCompletionTime()
    {
        if (completionTimeText != null)
        {
            completionTimeText.text = "Congratulations!!\nYou completed this level\nin " + ((Mathf.FloorToInt(elapsedTime / 60) == 0)? "": Mathf.FloorToInt(elapsedTime / 60) + " minutes and ") + Mathf.FloorToInt(elapsedTime % 60) + " seconds.";
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60); 
        int seconds = Mathf.FloorToInt(time % 60); 
        return $"{minutes:D2}:{seconds:D2}"; // 
    }
}