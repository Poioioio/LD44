using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    public float viewRadius=5f;
    public bool is2D = true;

    [Range(0, 360)]
    public float viewAngle=90f;
    
    public LayerMask obstacleMask;

    // Checks if the obj's transform position is in view. Do not use for big objects with gigonormous colliders you might want to detect.
    // still need obj to have a collider though.
    public bool CanSee(GameObject obj)
    {
        if (Vector3.Distance(obj.transform.position, transform.position) > viewRadius)
        {
            Debug.Log("OUT OF VIEW RADIUS");
            return false;
        }

        Vector3 lookAtObj = obj.transform.position - transform.position;
        //Debug.DrawRay(transform.position, lookAtObj);

        Vector3 forward = Vector3.forward;
        if(is2D)
            forward = Vector3.right;

        float angle = Vector3.Angle(forward, lookAtObj);
        if (angle > viewAngle / 2)
        {
            Debug.Log("OUT OF VIEW ANGLE : " + angle + " from vectors " + forward + " and " + lookAtObj);
            return false;
        }
        //Debug.Log("IN VIEW RADIUS AND ANGLE");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, lookAtObj, out hit, lookAtObj.magnitude, obstacleMask))
        {
            //Debug.Log("OBSTACLE IN THE WAY");
            return false;
        }
        Debug.Log("final result : obj in view !");

        return true;
    }
}
