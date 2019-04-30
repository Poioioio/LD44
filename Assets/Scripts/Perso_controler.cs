using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Collider2D attackCollider;
    public ContactFilter2D attackFilter;

    public float bumpForce = 100f;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public AudioClip pelleSound;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rigid = GetComponent<Rigidbody2D>();
        flipper = GetComponent<Flipper>();
        anim = GetComponentInChildren<Animator>();

        inventory["CopperCoin"] = 0;
        inventory["SilverCoin"] = 0;
        inventory["GoldCoin"] = 0;

        //HUD.GetInstance().UpdateCopperCoin(inventory["CopperCoin"]);
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
        bool inputJump = grounded && Input.GetAxis("Jump") > Mathf.Epsilon;
        anim.SetBool("Jump", inputJump);

        bool inputFire = grounded && Input.GetAxis("Fire1") > Mathf.Epsilon;
        anim.SetBool("Attack", inputFire);

        bool inputFire2 = grounded && Input.GetAxis("Fire2") > Mathf.Epsilon;
        anim.SetBool("Rob", inputFire2);
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
                HUD.GetInstance().UpdateCopperCoin(inventory["CopperCoin"]);
            }
            else if (collision.name == "SilverCoin")
            {
                HUD.GetInstance().UpdateSilverCoin(inventory["SilverCoin"]);
            }
            else if (collision.name == "GoldCoin")
            {
                HUD.GetInstance().UpdateGoldCoin(inventory["GoldCoin"]);
            }
        }
        else if( collision.gameObject.CompareTag("Projectile"))
        {            
            TakeDamageFrom(null, true, 0f);
        }
    }

    public override void TakeDamageFrom(AbstractController enmy, bool bumpRight, float force)
    {
        if (isActiveAndEnabled)
        {
            rigid.velocity = Vector2.zero;
            Vector2 bumpDir = bumpRight ? Vector2.right : Vector2.left;
            rigid.AddForce(bumpDir * force);
            bool dead = !HasCoins();
            if (inventory["CopperCoin"] > 0)
                DropCoppers();
            else if (inventory["SilverCoin"] > 0)
                DropSilvers();
            else if (inventory["GoldCoin"] > 0)
                DropGolds();

            if (dead)
            {
                anim.SetBool("Dead", true);
                audioSource.PlayOneShot(deathSound);
                enabled = false;
            }
            else
            {
                audioSource.PlayOneShot(hurtSound);
            }
            anim.SetTrigger("Hurt");
        }
    }

    bool HasCoins()
    {
        return ((inventory["CopperCoin"] > 0)
            || (inventory["SilverCoin"] > 0)
            || (inventory["GoldCoin"] > 0));
    }

    void DropCoppers()
    {
        Debug.Log("DropCoppers : ");
        inventory["CopperCoin"] = 0;
        HUD.GetInstance().UpdateCopperCoin(0);
    }

    void DropSilvers()
    {
        Debug.Log("DropSilvers");
        inventory["SilverCoin"] = 0;
        HUD.GetInstance().UpdateSilverCoin(0);
    }

    void DropGolds()
    {
        Debug.Log("DropGolds");
        inventory["GoldCoin"] = 0;
        HUD.GetInstance().UpdateGoldCoin(0);
    }


    public override void HitAnimTrigger()
    {
        List<Collider2D> targetList = new List<Collider2D>();
        audioSource.PlayOneShot(pelleSound);

        if (0 < Physics2D.OverlapCollider(attackCollider, attackFilter, targetList))
        {
            foreach (Collider2D target in targetList)
                target.gameObject.GetComponent<AbstractController>().TakeDamageFrom(this, flipper.facingRight, bumpForce);
        }
    }

    public override void JumpAnimTrigger()
    {
        rigid.AddForce(Vector2.up*jumpForce);
    }

    public override void DeathAnimTrigger()
    {
        //game Over text. R to restart
    }
}
