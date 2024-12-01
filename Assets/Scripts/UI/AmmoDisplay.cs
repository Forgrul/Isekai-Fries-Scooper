using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    private TMP_Text ammoText;   
    private PlayerController playerObject; 

    private void Start()
    {
        ammoText = GetComponentInChildren<TMP_Text>();
        playerObject = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        ammoText.text = "X" + playerObject.GetAmmo();
    }
}
