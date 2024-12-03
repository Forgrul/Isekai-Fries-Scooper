using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : EnemyBullet
{
    protected override void Start()
    {
        base.Start();
        StartCoroutine(FlipY());
    }

    IEnumerator FlipY()
    {
        while(true)
        {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            yield return new WaitForSeconds(0.4f);
        }
    }
}
