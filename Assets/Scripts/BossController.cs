using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : AbstractEnemyController
{
    public float callCooldown;
    float timeSinceLastCall;

    public AudioClip callSound;

    public List<GameObject> waves;
    int waveIndex = 0;

    protected override void Start()
    {
        timeSinceLastCall = callCooldown;

        base.Start();
    }

    private void Update()
    {
        if (waveIndex < waves.Count)
        {
            timeSinceLastCall += Time.deltaTime;

            if (timeSinceLastCall > callCooldown)
            {
                anim.SetBool("Call", true);
            }
        }
    }

    public override void AbilityAnimTrigger()
    {
        audioSource.PlayOneShot(callSound);
        waves[waveIndex].SetActive(true);
        waveIndex++;
        timeSinceLastCall = 0f;
        anim.SetBool("Call", false);
    }

}
