using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perso_controler : MonoBehaviour
{

    public float maxSpeed = 10f;

    Flipper flipper;
    Rigidbody2D rigid;
    Animator anim;

    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;

    public float jumpForce = 700f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        flipper = GetComponent<Flipper>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        anim.SetFloat("vSpeed", rigid.velocity.y);

        float move = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(move));

        rigid.velocity = new Vector2(move * maxSpeed, rigid.velocity.y);

        if ((move > 0 && !flipper.facingRight) || (move < 0 && flipper.facingRight))
        {
            flipper.Flip();
        }
    }

    void Update()
    {
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Ground", false);

            rigid.AddForce(new Vector2(0, jumpForce));
        }
    }

}
