using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform tar;           

    void LateUpdate()
    {
        if (tar != null)
        {
            transform.position = new Vector3(tar.position.x, transform.position.y, transform.position.z);
        }
    }
}