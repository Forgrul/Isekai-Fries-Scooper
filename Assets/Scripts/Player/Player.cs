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

    //sound effects
    public AudioSource audioSource; // 音源
    public AudioClip hurtPlayer; // 傷害音效
    // ----------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 初始化音源
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void GetHit()
    {
        if (isInvincible)
            return;

        currentHearts -= 1;

        // 播放受傷音效
        if (hurtPlayer != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurtPlayer);
        }
        else
        {
            Debug.LogWarning("Hurt sound or audio source is missing!");
        }


        if(currentHearts <= 0)
        {
            gameObject.GetComponent<Renderer>().enabled = false; 
            GameManager.Instance.showEnd(false);
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

    public bool IsInvincible()
    {
        return isInvincible;
    }
}
