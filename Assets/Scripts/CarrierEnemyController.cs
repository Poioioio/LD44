using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierEnemyController : AbstractEnemyController
{
    public bool loaded;
    public Magpie magpie;
    public Transform handTransform;
    public float cooldown = 2f;
    float timeSinceLoad = 2f;
    public float maxRange = 8;


    void Update()
    {
        if (!protag.isActiveAndEnabled)
        {
            enabled = false;
            return;
        }

        flipper.LookAt(protag.gameObject);

        if (loaded)
        {
            timeSinceLoad += Time.deltaTime;
            if (timeSinceLoad >= cooldown)
            {
                if (Vector2.Distance(protag.transform.position, transform.position) < maxRange)
                    Shoot();
            }
        }
    }

    void Shoot()
    {
        loaded = false;
        anim.SetBool("Loaded", loaded);
        timeSinceLoad = 0f;
        Debug.Log("shoot " + transform.localScale);
        magpie.Release(handTransform.position, transform.localScale);
    }

    void Reload()
    {
        loaded = true;
        anim.SetBool("Loaded", loaded);
        timeSinceLoad = 0f;
    }

    public override void TakeDamageFrom(AbstractController enmy, bool bumpRight, float force)
    {
        if (loaded)
            magpie.Release(handTransform.position, transform.localScale);

        magpie.Flee();

        base.TakeDamageFrom(enmy, bumpRight, force);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == magpie.gameObject)
        {
            if (magpie.Catched())
                Reload();
        }
    }
}