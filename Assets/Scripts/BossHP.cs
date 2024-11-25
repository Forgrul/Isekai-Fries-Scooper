using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    public Slider slider;      
    private Boss boss; 
    private float initialValue; 


    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        boss = FindObjectOfType<Boss>();
        slider.maxValue = 1f; 
        initialValue = boss.health; 
    }

    void Update()
    {
        UpdateSlider(boss.health);
    }

    void UpdateSlider(float currentValue)
    {
        float ratio = Mathf.Clamp(currentValue / initialValue, 0f, 1f); // 確保比值在 [0, 1] 範圍內
        slider.value = ratio; 
    }
}
