using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class galionControler : MonoBehaviour
{

    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
            else
            {
                source.enabled = true;
                source.Play();
            }
        }
    }
}
