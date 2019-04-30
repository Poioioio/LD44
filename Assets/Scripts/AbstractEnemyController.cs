using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnemyController : AbstractController
{
    public float maxSpeed = .8f;


    public AudioClip hurtSound;
    public AudioClip deathSound;

    protected Rigidbody2D rigid;
    protected Animator anim;
    protected Flipper flipper;
    protected Seeker seeker;
    protected WakeUpController wkCtrlr;
    protected SpriteEventManager spriteEventManager;
    protected SpriteRenderer spriteRenderer;

    protected Perso_controler protag;

    public int life = 2;
    protected bool grounded = false;
    protected bool playerInAttackRange = false;
    
    protected float myWidth;
    protected float myHeight;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        flipper = GetComponent<Flipper>();
        seeker = GetComponentInChildren<Seeker>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        myWidth = spriteRenderer.bounds.extents.x;
        myHeight = spriteRenderer.bounds.extents.y;

        wkCtrlr = GetComponent<WakeUpController>();
        protag = FindObjectOfType<Perso_controler>();
        spriteEventManager = GetComponentInChildren<SpriteEventManager>();
    }

    public override void TakeDamageFrom(AbstractController enmy, bool bumpRight, float force)
    {
        if (anim.GetBool("Dead") == false)
        {
            if (rigid != null)
            {
                rigid.velocity = Vector2.zero;
                Vector2 bumpDir = bumpRight ? Vector2.right : Vector2.left;
                rigid.AddForce(bumpDir * force);
            }

            life--;
            if (life <= 0)
            {
                audioSource.PlayOneShot(deathSound);
                anim.SetBool("Dead", true);
                enabled = false;
                wkCtrlr.enabled = false;
                GameObject empty = new GameObject();
                empty.transform.position = transform.position;
                spriteRenderer.transform.SetParent(empty.transform, true);

                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
                audioSource.PlayOneShot(hurtSound);
                enabled = true;
                wkCtrlr.enabled = false;
            }
            anim.SetTrigger("Hurt");
        }
    }
}
