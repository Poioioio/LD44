using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaCphantom_controler : AbstractController
{
    public float maxSpeed = .8f;

    Rigidbody2D rigid;
    Animator anim;
    Flipper flipper;
    Seeker seeker;

    public Collider2D attackCollider;
    public ContactFilter2D attackFilter;

    Perso_controler protag;
    
    bool grounded = false;
    bool playerInAttackRange = false;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
    float myWidth;
    float myHeight;

    public float sight_distance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        flipper = GetComponent<Flipper>();
        anim = GetComponentInChildren<Animator>();
        seeker = GetComponentInChildren<Seeker>();
        myWidth = GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
        myHeight = GetComponentInChildren<SpriteRenderer>().bounds.extents.y;

        protag = FindObjectOfType<Perso_controler>();
        enabled = false;
    }

    void FixedUpdate()
    {
        if( !protag.isActiveAndEnabled )
        {
            enabled = false;
            return;
        }

        flipper.LookAt(protag.gameObject);

        // Check if we are at the end of the plateforme
        Vector2 linecast_ground = transform.position;// - transform.right * myWidth;
        Debug.DrawLine(linecast_ground, linecast_ground + Vector2.down);
        grounded = Physics2D.Linecast(linecast_ground, linecast_ground + Vector2.down, whatIsGround);
        anim.SetBool("Ground", grounded);

        // Check if player is seen
        Vector2 linecast_player = transform.position + transform.up * myHeight;
        Debug.DrawLine(linecast_player, linecast_player + Vector2.right * myWidth);
        playerInAttackRange = Physics2D.Linecast(linecast_player, linecast_player + flipper.GetForward2D() * myWidth*2, whatIsPlayer);

        anim.SetFloat("vSpeed", rigid.velocity.y);

        //float move = Input.GetAxis("Horizontal");
        float move = 0f;

        if (grounded && !playerInAttackRange)
        {
            move = flipper.facingRight ? 1 : -1;

            anim.SetFloat("Speed", Mathf.Abs(move));
            rigid.velocity = new Vector2(move * maxSpeed, rigid.velocity.y);
        }

        if (playerInAttackRange)
        {
            anim.SetTrigger("Attack");
        }

    }

    public override void HitAnimTrigger()
    {
        Collider2D[] target = new Collider2D[1];

        if( 1 == Physics2D.OverlapCollider(attackCollider, attackFilter, target))
        {
            target[0].gameObject.GetComponent<Perso_controler>().TakeDamageFrom(this);
        }
    }

}
