using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEventManager : MonoBehaviour
{
    AbstractController ctrl;
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
        Debug.Log("Sptr");
        ctrl.JumpAnimTrigger();
    }

    public void CallDeathAnimTrigger()
    {
        ctrl.DeathAnimTrigger();
    }
}
