using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAimPoint : MonoBehaviour
{
    public Rigidbody2D protag;
    public float lookForwardFactor = 3f;

    public Transform leftBorder;
    public Transform rightBorder;
    public Transform bottomBorder;
    public Transform topBorder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 unclampedAim = protag.position + protag.velocity.normalized * lookForwardFactor;

        Vector3 minXY = Camera.main.ViewportToWorldPoint(new Vector3(-1, -1, 0));
        Vector3 maxXY = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 diff = (maxXY - minXY)/4f;

        Vector2 clampedAim = unclampedAim;

        if (unclampedAim.y < bottomBorder.transform.position.y + diff.y )
            clampedAim.y = bottomBorder.transform.position.y + diff.y;

        if (unclampedAim.y > topBorder.transform.position.y - diff.y)
            clampedAim.y = topBorder.transform.position.y - diff.y;

        if (unclampedAim.x < leftBorder.transform.position.x + diff.x)
            clampedAim.x = leftBorder.transform.position.x + diff.x;

        if (unclampedAim.x > rightBorder.transform.position.x - diff.x)
            clampedAim.x = rightBorder.transform.position.x - diff.x;

        transform.position = new Vector3(clampedAim.x, clampedAim.y, 0);
    }
}
