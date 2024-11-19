using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectArea : MonoBehaviour
{
    bool canRotate = true;
    bool isDeflecting = false;
    float angle;

    // Update is called once per frame
    void Update()
    {
        if(canRotate)
            UpdateDirection();
        
        // Since its scale is bound to its parent...
        UpdateRotationBasedOnParentScale();
    }

    void UpdateDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 direction = mousePosWorld - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void UpdateRotationBasedOnParentScale()
    {
        if(Mathf.Sign(transform.parent.localScale.x) == 1)
            transform.rotation = Quaternion.Euler(0, 0, angle);
        else
            transform.rotation = Quaternion.Euler(0, 0, 180 + angle);
    }

    public void StartDeflect()
    {
        // GetComponent<Collider2D>().enabled = true;
        canRotate = false;
        isDeflecting = true;
    }

    public void StopDeflect()
    {
        // GetComponent<Collider2D>().enabled = false;
        canRotate = true;
        isDeflecting = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!isDeflecting)
            return;
        // if is enemy bullet: bullet.Deflect();
        if(other.gameObject.CompareTag("Bullet"))
        {
            float angle = transform.rotation.eulerAngles.z;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
            other.gameObject.GetComponent<Bullet>().Deflect(direction);
        }
    }
}
