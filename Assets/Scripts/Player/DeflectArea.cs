using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectArea : MonoBehaviour
{
    public PlayerController player;
    bool canRotate = true;
    float angle;

    // animation
    Animator anim;
    int deflectAnim;

    void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        anim = transform.GetChild(0).GetComponent<Animator>();
        deflectAnim = Animator.StringToHash("deflect");
    }

    void Update()
    {
        transform.position = player.transform.position;
        if(canRotate)
            UpdateDirection();
    }

    void UpdateDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 direction = mousePosWorld - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public float StartDeflect()
    {
        anim.SetTrigger(deflectAnim);
        GetComponent<Collider2D>().enabled = true;
        canRotate = false;
        return angle;
    }

    public void StopDeflect()
    {
        GetComponent<Collider2D>().enabled = false;
        canRotate = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Bullet"))
        {
            EnemyBullet bulletScript = other.gameObject.GetComponent<EnemyBullet>();
            if(bulletScript == null) 
                return;

            float angle = transform.rotation.eulerAngles.z;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
            bulletScript.Deflect(direction);
        }
    }
}
