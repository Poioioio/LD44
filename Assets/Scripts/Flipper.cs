using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public bool facingRight = true;
    public float margin = 0.2f;

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public Vector3 GetForward()
    {
        if (facingRight)
            return Vector3.right;
        else
            return Vector3.left;
    }

    public Vector2 GetForward2D()
    {
        if (facingRight)
            return Vector2.right;
        else
            return Vector2.left;
    }
    public void LookAt(GameObject target)
    {
        float diff = target.transform.position.x - transform.position.x;
        if ((diff > margin && !facingRight)
            || (diff < -margin && facingRight))
            Flip();
    }
}
