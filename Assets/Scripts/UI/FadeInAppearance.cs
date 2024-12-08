using UnityEngine;
using System.Collections;

public class FadeInAppearance : MonoBehaviour
{
    public float delayTime = 0.0f; 
    public float fadeDuration = 1.0f; 
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
        
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(delayTime);
        
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            spriteRenderer.color = color;
            yield return null;
        }

        color.a = 1;
        spriteRenderer.color = color;
    }
}
