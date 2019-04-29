using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perso_controler : AbstractController
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

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public Text copperText;
    public Image copperImage;
    public Text silverText;
    public Image silverImage;
    public Text goldText;
    public Image goldImage;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        flipper = GetComponent<Flipper>();
        anim = GetComponentInChildren<Animator>();

        inventory["CopperCoin"] = 3;
        inventory["SilverCoin"] = 0;
        inventory["GoldCoin"] = 0;
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
        if(grounded && Input.GetAxis("Jump")> Mathf.Epsilon)//Input.GetKeyDown(KeyCode.Space))
            anim.SetBool("Jump",true);
        else
            anim.SetBool("Jump", false);

        if ( grounded && Input.GetAxis("Fire1") > Mathf.Epsilon)
            anim.SetBool("Attack",true);
        else
            anim.SetBool("Attack", false);

        if (grounded && Input.GetAxis("Fire2") > Mathf.Epsilon)
            anim.SetBool("Rob", true);
        else
            anim.SetBool("Rob", false);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);

            if(inventory.ContainsKey(collision.name))
            {
                inventory[collision.name] += 1;
            }
            else
            {
                inventory.Add(collision.name, 1);
            }

            if (collision.name == "CopperCoin")
            {
                copperText.gameObject.SetActive(true);
                copperImage.gameObject.SetActive(true);
                copperText.text = "Tibet coins : " + inventory["CopperCoin"].ToString();
            }
            else if (collision.name == "SilverCoin")
            {
                silverText.gameObject.SetActive(true);
                silverImage.gameObject.SetActive(true);
                silverText.text = "Drachmes : " + inventory["SilverCoin"].ToString();
            }
            else if (collision.name == "GoldCoin")
            {
                goldImage.gameObject.SetActive(true);
                goldText.gameObject.SetActive(true);
                goldText.text = "Stateres : " + inventory["GoldCoin"].ToString();
            }
        }
    }

    public override void TakeDamageFrom(AbstractController enmy, bool bumpRight, float force)
    {
        if (isActiveAndEnabled)
        {
            rigid.velocity = Vector2.zero;
            Vector2 bumpDir = bumpRight ? Vector2.right : Vector2.left;
            rigid.AddForce(bumpDir * force);
            if (inventory["CopperCoin"] > 0)
                DropCoppers();
            else if (inventory["SilverCoin"] > 0)
                DropSilvers();
            else if (inventory["GoldCoin"] > 0)
                DropGolds();
            else
            {
                anim.SetBool("Dead", true);
                enabled = false;
            }
            anim.SetTrigger("Hurt");
        }
    }

    void DropCoppers()
    {
        Debug.Log("DropCoppers : ");
        inventory["CopperCoin"] = 0;

    }

    void DropSilvers()
    {
        Debug.Log("DropSilvers");
        inventory["SilverCoin"] = 0;

    }

    void DropGolds()
    {
        Debug.Log("DropGolds");
        inventory["GoldCoin"] = 0;

    }


    public override void HitAnimTrigger()
    {
        Collider2D[] target = new Collider2D[1];
        Debug.Log("aaa");

        if (1 == Physics2D.OverlapCollider(attackCollider, attackFilter, target))
        {
            Debug.Log("zzzz");

            target[0].gameObject.GetComponent<AbstractController>().TakeDamageFrom(this, flipper.facingRight, bumpForce);
        }
    }

    public override void JumpAnimTrigger()
    {
        Debug.Log("JUMP");
        rigid.AddForce(new Vector2(0, jumpForce));
    }

    public override void DeathAnimTrigger()
    {
        //game Over text. R to restart
    }
}
