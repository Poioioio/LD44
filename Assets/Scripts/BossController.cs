using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : AbstractEnemyController
{
    public float callCooldown;
    float timeSinceLastCall;

    protected override void Start()
    {
        timeSinceLastCall = callCooldown;

        base.Start();
    }

    private void Update()
    {
        timeSinceLastCall += Time.deltaTime;

        if( timeSinceLastCall > callCooldown )
        {
            anim.SetBool("Call", true);
        }
    }

    public override void AbilityAnimTrigger()
    {
        anim.SetBool("Call", true);
    }


}
