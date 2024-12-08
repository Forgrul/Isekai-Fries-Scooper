using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleSwitch : MonoBehaviour
{
    private Image buttonImage;
    private TMP_Text buttonText;   

    void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TMP_Text>();
        UpdateButtonColor();
    }

    public void Toggle()
    {
        UpdateButtonColor();
    }

    private void UpdateButtonColor()
    {
        if (GameManager.Instance.GetHardcoreMode()) 
        {
            buttonImage.color = Color.red; 
            buttonText.text = "HardCore : ON";
        }
        else 
        {
            buttonImage.color = Color.green;
            buttonText.text = "HardCore : OFF";
        }
    }
}
