using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Staminabar : MonoBehaviour
{
    public Slider staminaSlider; 

    private PlayerController playerObject; 


    void Start()
    {
        staminaSlider = GetComponentInChildren<Slider>();
        playerObject = FindObjectOfType<PlayerController>();

        staminaSlider.maxValue = playerObject.maxStamina;
        staminaSlider.value = playerObject.maxStamina;
    }

    private void Update()
    {
        staminaSlider.value = playerObject.GetStamina();
    }
}
