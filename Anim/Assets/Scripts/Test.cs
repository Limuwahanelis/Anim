using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Test : MonoBehaviour
{
    [SerializeField] Transform rayStart;
    [SerializeField] Transform rayStartLeft;
    [SerializeField] Transform rayStartRight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(rayStart.position, rayStart.forward);
        Ray rayLeft = new Ray(rayStartLeft.position, rayStartLeft.forward);
        Ray rayRight = new Ray(rayStartRight.position, rayStartRight.forward);
        RaycastHit hit;
        RaycastHit hitLeft;
        RaycastHit hitRight;
        Physics.Raycast(rayLeft, out hitLeft, 3f);
        Physics.Raycast(rayRight, out hitRight, 3f);
        Quaternion rot = Quaternion.identity;
        if (Physics.Raycast(ray, out hit, 3f))
        {
                
            //transform.forward = -hit.normal;
        }
        if (hitLeft.normal == hitRight.normal && hit.normal == hitLeft.normal && hit.normal == hitRight.normal)
        {
            rot.SetLookRotation( -hit.normal,Vector3.up);
            transform.rotation = rot;

        }
        

    }
}
