using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackTime = 0.25f;

    public DeflectArea attackArea;

    bool isAttacking = false;
    float timer;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            Attack();
        
        if(isAttacking)
        {
            timer += Time.deltaTime;
            if(timer >= attackTime)
                StopAttack();
        }
    }

    void Attack()
    {
        attackArea.StartDeflect();
        isAttacking = true;

        timer = 0f;
    }

    void StopAttack()
    {
        attackArea.StopDeflect();
        isAttacking = false;
    }
}
