using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    public float viewRadius=5f;
    public bool is2D = true;
    Flipper flipper;
    [Range(0f, 360f)]
    public float viewAngle=90f;
    
    public LayerMask obstacleMask;

    private void Start()
    {
        flipper = GetComponentInParent<Flipper>();
    }

    private void Update()
    {
        CanSee(null);//good way to see the debug rays;
    }

    // Checks if the obj's transform position is in view. Do not use for big objects with gigonormous colliders you might want to detect.
    // still need obj to have a collider though.
    public bool CanSee(GameObject obj)
    {
        Vector3 forward = Vector3.forward;
        if(is2D)
        {
            forward = transform.rotation * flipper.GetForward();
            Debug.DrawRay(transform.position, forward, Color.red, 1f);
            Debug.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -viewAngle / 2f) * forward * viewRadius, Color.black, 1f);
            Debug.DrawRay(transform.position, Quaternion.Euler(0f, 0f, viewAngle / 2f) * forward * viewRadius, Color.black, 1f);
        }

        if (obj == null)
            return false;

        if (Vector3.Distance(obj.transform.position, transform.position) > viewRadius)
        {
            //Debug.Log("OUT OF VIEW RADIUS");
            return false;
        }

        Vector3 lookAtObj = obj.transform.position - transform.position;
        //Debug.DrawRay(transform.position, lookAtObj);

        
        float angle = Vector3.Angle(forward, lookAtObj);
        if (angle > viewAngle / 2f)
        {
            //Debug.Log("OUT OF VIEW ANGLE : " + angle + " from vectors " + forward + " and " + lookAtObj);
            return false;
        }
        //Debug.Log("IN VIEW RADIUS AND ANGLE");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, lookAtObj, out hit, lookAtObj.magnitude, obstacleMask))
        {
            //Debug.Log("OBSTACLE IN THE WAY");
            return false;
        }
        //Debug.Log("final result : obj in view !");

        return true;
    }
}
