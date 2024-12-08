using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : EnemyBullet
{

    public string resourceFolder = "Fireball_Anim";
    public float frameRate = 0.01f;

    private Sprite[] bulletSprites;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {

        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletSprites = Resources.LoadAll<Sprite>(resourceFolder);
        StartCoroutine(PlayAnimation());

        // StartCoroutine(FlipY());
    }

    // IEnumerator FlipY()
    // {
    //     while(true)
    //     {
    //         transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
    //         yield return new WaitForSeconds(0.4f);
    //     }
    // }
    private IEnumerator PlayAnimation()
    {
        int index = 0;

        while (true)
        {
            // transform.localScale = new Vector3(2f, 2f, 1f);
            spriteRenderer.sprite = bulletSprites[index];
            yield return new WaitForSeconds(frameRate);
            index = (index + 1) % bulletSprites.Length;
        }
    }

    protected override void SetUpHardcore()
    {
        bounceCountMax++;
    }
}
