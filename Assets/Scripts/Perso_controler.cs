using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perso_controler : MonoBehaviour
{

    public float maxSpeed = 10f;
    public bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");

        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>() ;

        rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

        if ((move > 0 && !facingRight) || (move < 0 && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
