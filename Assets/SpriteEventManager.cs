using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEventManager : MonoBehaviour
{
    AbstractController ctrl;
    public bool destroyParentAtTheEnd = true;//useful to destroy rigidbody at the beginning of the anim and the sprite at the end of it.

    private void Start()
    {
        ctrl = GetComponentInParent<AbstractController>();
    }
    public void CallHitAnimTrigger()
    {
        ctrl.HitAnimTrigger();
    }

    public void CallJumpAnimTrigger()
    {
        ctrl.JumpAnimTrigger();
    }

    public void CallDeathAnimTrigger()
    {
        if( ctrl != null)
            ctrl.DeathAnimTrigger();

        if(destroyParentAtTheEnd)
            Destroy(transform.parent.gameObject);
    }
}
