using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AbstractController : MonoBehaviour
{

    public Collider2D attackCollider;
    public ContactFilter2D attackFilter;

    public float bumpForce = 100f;

    virtual public void HitAnimTrigger() { }
    virtual public void JumpAnimTrigger() { Debug.Log("Abstr"); }
    virtual public void DeathAnimTrigger() { }
    virtual public void TakeDamageFrom(AbstractController enmy, bool bumpRight, float force) { }

}
