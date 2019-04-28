using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpController : MonoBehaviour
{
    public MonoBehaviour controller;
    Seeker seeker;
    GameObject protag;

    // Start is called before the first frame update
    void Start()
    {
        protag = FindObjectOfType<Perso_controler>().gameObject;
        seeker = GetComponentInChildren<Seeker>();
    }

    // Update is called once per frame
    void Update()
    {
        if( seeker.CanSee( protag ) )
        {
            controller.enabled = true;
            enabled = false;
        }
    }
}
