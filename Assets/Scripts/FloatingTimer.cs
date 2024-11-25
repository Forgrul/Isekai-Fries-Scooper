using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FloatingTimer : MonoBehaviour
{
    public Slider countdownSlider; 

    private Coroutine countdownCoroutine;

    void Start()
    {
        countdownSlider = GetComponentInChildren<Slider>();
        gameObject.SetActive(false);    
    }

    public void StartTimer(float duration)
    {
        // 如果有正在執行的倒計時，先停止
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        countdownSlider.maxValue = duration;
        countdownSlider.value = duration;

        countdownCoroutine = StartCoroutine(CountdownRoutine(duration));
    }

    private IEnumerator CountdownRoutine(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float remainingTime = Mathf.Clamp(duration - elapsedTime, 0f, duration);

            countdownSlider.value = remainingTime;

            yield return null; 
        }

        OnCountdownEnd();
    }

    private void OnCountdownEnd()
    {
        countdownSlider.value = 0; 
        gameObject.SetActive(false);
    }
}
