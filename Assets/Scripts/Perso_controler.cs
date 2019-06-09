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

    private Dictionary<Item.Type, List<Item>> inventory = new Dictionary<Item.Type, List<Item>>();

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

        if( Input.GetKeyUp(KeyCode.C))
        {
            TakeDamageFrom(null, false, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            collision.gameObject.SetActive(false);
            Item item = collision.gameObject.GetComponent<Item>();
            if (item == null)
                return;

            if(inventory.ContainsKey(item.type))
            {
                inventory[item.type].Add(item);
            }
            else
            {
                List<Item> list = new List<Item>();
                list.Add(item);
                inventory.Add(item.type, list);
            }

            switch( item.type )
            {
                case Item.Type.copperCoin:
                    HUD.GetInstance().UpdateCopperCoin(inventory[item.type].Count);
                    break;
                case Item.Type.silverCoin:
                    HUD.GetInstance().UpdateSilverCoin(inventory[item.type].Count);
                    break;
                case Item.Type.goldCoin:
                    HUD.GetInstance().UpdateGoldCoin(inventory[item.type].Count);
                    break;
                default:
                    break;
            }
        }
        else if( collision.gameObject.CompareTag("Projectile"))
        {            
            TakeDamageFrom(null, true, 0f);
        }
        else if (collision.gameObject.CompareTag("Defeat"))
        {
            HUD.GetInstance().ShowGameOver();
        }
        else if (collision.gameObject.CompareTag("Victory"))
        {
            HUD.GetInstance().ShowVictory();
        }

    }

    public override void TakeDamageFrom(AbstractController enmy, bool bumpRight, float force)
    {
        if (isActiveAndEnabled)
        {
            rigid.velocity = Vector2.zero;
            Vector2 bumpDir = bumpRight ? Vector2.right : Vector2.left;
            rigid.AddForce(bumpDir * force);
            Item.Type dropType = Item.Type.NONE;
            for( int i = (int)Item.Type.copperCoin; i <= (int)Item.Type.goldCoin;++i )
            {
                Item.Type type = (Item.Type)i;
                if (Has(type))
                {
                    dropType = type;
                }
            }

            if( dropType != Item.Type.NONE )
            {
                DropAllItemsOfType(dropType);
                audioSource.PlayOneShot(hurtSound);
            }
            else
            {
                anim.SetBool("Dead", true);
                audioSource.PlayOneShot(deathSound);
                enabled = false;
            }
            anim.SetTrigger("Hurt");
        }
    }

    bool Has(Item.Type type )
    {
        return (inventory.ContainsKey(type)) && (inventory[type].Count > 0);
    }

    void DropAllItemsOfType( Item.Type type )
    {
        foreach (Item item in inventory[type])
            item.Drop( transform.position );

        inventory[type].Clear();

        switch ( type )
        {
            case Item.Type.copperCoin:
                HUD.GetInstance().UpdateCopperCoin(0); break;
            case Item.Type.silverCoin:
                HUD.GetInstance().UpdateSilverCoin(0); break;
            case Item.Type.goldCoin:
                HUD.GetInstance().UpdateGoldCoin(0); break;
            default: break;
        }
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
        HUD.GetInstance().ShowGameOver( true );
    }
}
