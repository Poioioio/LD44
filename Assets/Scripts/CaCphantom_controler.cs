using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaCphantom_controler : MonoBehaviour
{
    public float maxSpeed = .8f;

    Rigidbody2D rigid;
    Animator anim;
    Flipper flipper;
    Seeker seeker;
    
    bool grounded = false;
    bool playerInSight = false;
    bool playerInRange = false;
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
    }

    void FixedUpdate()
    {
        // Check if we are at the end of the plateforme
        Vector2 linecast_ground = transform.position - transform.right * myWidth;
        Debug.DrawLine(linecast_ground, linecast_ground + Vector2.down);
        grounded = Physics2D.Linecast(linecast_ground, linecast_ground + Vector2.down, whatIsGround);
        anim.SetBool("Ground", grounded);

        // Check if player is seen
        Vector2 linecast_player = transform.position + transform.up * myHeight;
        Debug.DrawLine(linecast_player, linecast_player + Vector2.right * myWidth);
        playerInSight = Physics2D.Linecast(linecast_player, linecast_player + Vector2.right * sight_distance, whatIsPlayer);
        anim.SetBool("PlayerInSight", playerInSight);
        playerInRange = Physics2D.Linecast(linecast_player, linecast_player + Vector2.right * myWidth*2, whatIsPlayer);

        anim.SetFloat("vSpeed", rigid.velocity.y);

        //float move = Input.GetAxis("Horizontal");
        float move = 0f;

        if (playerInSight && grounded && !playerInRange)
        {
            move = flipper.facingRight ? 1 : -1;
        }

        if (playerInRange)
        {
            anim.SetTrigger("Attack");
        }

        anim.SetFloat("Speed", Mathf.Abs(move));

        rigid.velocity = new Vector2(move * maxSpeed, rigid.velocity.y);

        if ((move > 0 && !flipper.facingRight) || (move < 0 && flipper.facingRight))
        {
            Debug.Log("fLIP");
            flipper.Flip();
        }
    }

}
