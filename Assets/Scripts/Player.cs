using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{


    public int maxHearts = 5; // 總愛心數量
    private int currentHearts;

    public GameObject heartPrefab; // 愛心預製件
    public Transform canvasTransform; // Canvas 作為愛心的父物件
    [SerializeField] private Vector2 startPosition = new Vector2(-300, 250); // 第一顆愛心的位置
    [SerializeField] private float heartSpacing = 80f; // 每顆愛心的水平間距
    private List<GameObject> hearts = new List<GameObject>();
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {

        currentHearts = maxHearts;
        UpdateHeartsUI();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void GetHit(int dmg)
    {
        if (isInvincible)
            return;


        currentHearts -= 1;
        if(currentHearts <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
            
        UpdateHeartsUI();
        StartCoroutine(InvincibilityCoroutine());
    }



    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // 閃爍效果
        float flashDuration = 0.1f; // 每次閃爍的時間
        for (int i = 0; i < 15; i++) // 總共閃爍 3 秒
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // 切換顯示
            yield return new WaitForSeconds(flashDuration);
        }

        spriteRenderer.enabled = true; // 確保最終是顯示狀態
        isInvincible = false;
    }
    private void UpdateHeartsUI()
    {
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        for (int i = 0; i < currentHearts; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, canvasTransform);
            hearts.Add(newHeart);

            // 設定每顆愛心的位置
            RectTransform heartTransform = newHeart.GetComponent<RectTransform>();
            if (heartTransform != null)
            {
                heartTransform.anchoredPosition = startPosition + new Vector2(i * heartSpacing, 0);
                heartTransform.localScale = new Vector3(0.7f, 0.7f, 1f);
            }
        }
    }
}
