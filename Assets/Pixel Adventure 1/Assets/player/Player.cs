using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHp = 100;
    int currentHp;
    public Image healthBarImage;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit(int dmg)
    {
        currentHp -= dmg;
        if(currentHp <= 0)
            Destroy(gameObject);
        healthBarImage.fillAmount = GetHealthPercentage();
        Debug.Log(GetHealthPercentage());
    }

    public float GetHealthPercentage()
    {
        return (float)currentHp / maxHp; // Return the percentage of current health
    }
}
