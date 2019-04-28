using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
