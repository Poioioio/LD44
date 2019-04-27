using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxed : MonoBehaviour
{
    public float speedFactor;
    Vector3 startCamPos;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startCamPos = Camera.main.transform.position;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diffCamPos = Camera.main.transform.position - startCamPos;
        transform.position = startPos - diffCamPos * speedFactor;
        
    }
}
