using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Fire Settings")]
    public GameObject slowBulletPrefab;
    public GameObject fastBulletPrefab;

    [Header("Spiral Attack")]
    public int spiralFireCount = 20;

    [Header("Diverge Attack")]
    public int divergeWaveCount = 5;
    public float waveInterval = 0.5f;
    public int divergeFireCount = 12;

    delegate IEnumerator Attack();

    protected override void Patrol()
    {
        
    }
    
    protected override void Fire()
    {
        Attack[] attacks = { SpiralAttack, DivergeAttack };
        int index = Random.Range(0, attacks.Length);
        Debug.Log(index);
        StartCoroutine(attacks[index]());
    }

    IEnumerator SpiralAttack()
    {
        for(int i = 0; i < spiralFireCount; i++)
        {
            float angle = 360f / spiralFireCount * i;

            for(int j = 0; j < 2; j++)
            {
                GameObject b = Instantiate(slowBulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
                Bullet bulletScript = b.GetComponent<Bullet>();
                bulletScript.SetDirectionAngle(angle);
                angle += 180f;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator DivergeAttack()
    {
        int divergeWaveCount = 5;
        float initPhase = Random.Range(0f, 360f);
        for(int i = 0; i < divergeWaveCount; i++)
        {
            int divergeFireCount = 12;
            float phase = initPhase + (360f / divergeFireCount) / 2 * i;
            for(int j = 0; j < divergeFireCount; j++)
            {
                float angle = 360f / divergeFireCount * j + phase;
                GameObject b = Instantiate(fastBulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
                Bullet bulletScript = b.GetComponent<Bullet>();
                bulletScript.SetDirectionAngle(angle);
            }

            yield return new WaitForSeconds(waveInterval);
        }
    }
}
