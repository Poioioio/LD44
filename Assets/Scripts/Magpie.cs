using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magpie : MonoBehaviour
{
    enum State {
        getHigh,
        dive,
        overshoot,
        getBackHigh,
        getBackToHand
    };

    State state = State.getHigh;
    Animator anim;

    bool flee = false;
    
    Vector2 normFleeDir;
    public float speed;

    public float overShootFactor = 2f;
    public Transform hand;
    public Vector3 targetPoint;
    Perso_controler protag;
    Vector2 dir;

    // Start is called before the first frame update
    void Start()
    {
        protag = FindObjectOfType<Perso_controler>();
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
        normFleeDir = Random.insideUnitCircle;
        normFleeDir.y = Mathf.Abs(normFleeDir.y);
    }


    private void Update()
    {
        dir = Vector2.zero;
        if ( flee )
        {
            dir = normFleeDir * speed * Time.deltaTime;
        }
        else
        {
            dir = new Vector2(targetPoint.x - transform.position.x,  targetPoint.y - transform.position.y);
            dir.Normalize();
            dir = dir * speed * Time.deltaTime;
        }       


        LookAt(dir);
        transform.position = transform.position + new Vector3(dir.x, dir.y, 0);

        if (!flee)
        {
            Vector2 checkDir = new Vector2(targetPoint.x - transform.position.x, targetPoint.y - transform.position.y);
            bool passedTarget = Vector2.Dot(checkDir, dir) < -Mathf.Epsilon;
            if (passedTarget)
            {
                if (state == State.getBackToHand)
                    state = 0;
                else state++;
                InitState();
            }
        }
    }

    void InitState()
    {
        //Debug.Log("initState :" + state);
        Vector3 dir3 = new Vector3(dir.x, dir.y, 0f);
        bool protagRight = protag.transform.position.x > transform.position.x;
        float protagSideFactor = protagRight ? 1 : -1;
        switch( state )
        {
            case State.getHigh:            targetPoint = transform.position + new Vector3(protagSideFactor*2f, 3f, 0f); break;
            case State.dive:               targetPoint = protag.transform.position; break;
            case State.overshoot:          targetPoint = transform.position + dir3 * overShootFactor; break;
            case State.getBackHigh:        targetPoint = transform.position + dir3 + Vector3.up * 5f; break;
            case State.getBackToHand:      targetPoint = hand.position; break;
            default:
                break;
        }
    }

    void LookAt(Vector2 dir)
    {
        if (dir.y >= Mathf.Epsilon)
            transform.eulerAngles = new Vector3(0f, 0f, 90f);
        else transform.eulerAngles = Vector3.zero;

        Vector3 newScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), 1f);
        if (dir.x < -Mathf.Epsilon)
        {
            if (dir.y >= Mathf.Epsilon)
                newScale.Scale(new Vector3(1f, -1f, 1f));
            else
                newScale.Scale(new Vector3(-1f, 1f, 1f));
        }

        transform.localScale = newScale;
    }

    public void Release( Vector3 startPosition, Vector3 scale )
    {
        gameObject.SetActive(true);
        GameObject empty = new GameObject();
        transform.position = startPosition;
        transform.SetParent(empty.transform, true);
        if (!flee)
        {
            state = State.getHigh;
            InitState();
        }
    }

    public bool Catched()
    {
        if (state == State.getBackToHand)
        {
            GameObject empty = transform.parent.gameObject;
            transform.SetParent(hand);
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    public void Flee()
    {        
        flee = true;
        anim.SetBool("Flee", true);
    }
    
}
