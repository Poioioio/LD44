using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScroller : MonoBehaviour
{
    public GameObject aimPoint;
    public float smoothSpeed = 0.125f;
    public Vector3 zOffset;

    // Start is called before the first frame update
    void Start()
    {
        aimPoint.transform.position = transform.position - zOffset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 aim = aimPoint.transform.position + zOffset;
        transform.position = Vector3.Lerp(transform.position, aim, smoothSpeed);
    }
}
