using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ffffs : MonoBehaviour
{
    bool isClimbing = false;
    bool inPosition;
    bool isLerping;
    float t;
    Vector3 startPos;
    Vector3 targetPos;
    Quaternion startRot;
    Quaternion targetRot;
    //public float possitionOffset;
    public float rayTowardsMoveDir;
    public float offsetFromWall = 0.3f;
    public float speed_multiplier = 0.2f;
    public float climbSpeed = 3f;
    public float rotateSpeed = 2f;
    public float rayForwadsWall = 1;
    Transform helper;
    float delta;
    public void CheckForClimb()
    {
        Vector3 origin = transform.position;
        origin.y += 1.4f;
        Vector3 dir = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, 5))
        {
            helper.transform.position = PosWithOffset(origin, hit.point);
            InitForClimb(hit);
        }

    }
    void Init()
    {
        helper = new GameObject().transform;
        helper.name = "climb_helper";
        CheckForClimb();
    }
    void InitForClimb(RaycastHit hit)
    {
        isClimbing = true;
        helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
        startPos = transform.position;
        targetPos = hit.point + (hit.normal * offsetFromWall);
        t = 0;
        inPosition = false;
        // anim stuff
    }
    void UpdateT()
    {
        delta = Time.deltaTime;
        Tick(delta);
    }
    void Tick(float delta)
    {
        if (!inPosition)
        {
            GetInPosition();
            return;
        }

        if (!isLerping)
        {
            // inputy

            Vector3 moveDir = Vector3.zero;// normalized hor i vert
            bool canMove = CanMove(moveDir);
            if (!canMove || moveDir == Vector3.zero) return;

            t = 0;
            isLerping = true;
            startPos = transform.position;
            //Vector3 tp = helper.position - transform.position;
            targetPos = helper.position;

        }
        else
        {
            t += delta * climbSpeed;
            if (t > 1)
            {
                t = 1;
                isLerping = false;
            }
            Vector3 cp = Vector3.Lerp(startPos, targetPos, t);
            transform.position = cp;
            transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
        }
    }
    bool CanMove(Vector3 moveDir)
    {
        Vector3 origin = transform.position;
        float dis = rayTowardsMoveDir; //possitionOffset
        Vector3 dir = moveDir;
        Debug.DrawRay(origin, dir * dis, Color.red);
        RaycastHit hit;

        //raycast towards the irection you want to move
        if (Physics.Raycast(origin, dir, out hit, dis))// checks if there is wall perpendicualar to us
        {
            //check if its a corner
            return false;
        }

        origin += moveDir * dis;
        dir = helper.forward;
        float dis2 = rayForwadsWall;

        //raycast forwards towards the wall
        Debug.DrawRay(origin, dir * dis2, Color.blue);
        if (Physics.Raycast(origin, dir, out hit, dis2))
        {
            helper.position = PosWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }

        origin = origin + (dir * dis2);
        dir = -moveDir;

        // raycast for around corners 
        if(Physics.Raycast(origin,dir,out hit,rayForwadsWall))
        {
            helper.position = PosWithOffset(origin, hit.point);
            helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }

        origin += dir * dis2;
        dir = -Vector3.up;

        if (Physics.Raycast(origin, dir, out hit, dis2))
        {
            float angle = Vector3.Angle(-helper.forward, hit.normal);
            if (angle < 40)
            {
                helper.position = PosWithOffset(origin, hit.point);
                helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }
        }

        //Debug.DrawRay(origin, dir, Color.yellow);
        //if (Physics.Raycast(origin, dir, out hit, dis2))
        //{
        //    float angle = Vector3.Angle(helper.up, hit.normal);
        //    if (angle < 40)
        //    {
        //        helper.position = PosWithOffset(origin, hit.point);
        //        helper.rotation = Quaternion.LookRotation(-hit.normal);
        //        return true;
        //    }
        //}

        return false;
    }
    void GetInPosition()
    {
        t += delta;
        if (t > 1)
        {
            t = 1;
            inPosition = true;
            // ik
        }
        Vector3 tp = Vector3.Lerp(startPos, targetPos, t);
        transform.position = tp;
        transform.rotation = Quaternion.Slerp(transform.rotation, helper.rotation, delta * rotateSpeed);
    }
    Vector3 PosWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * offsetFromWall;
        return target + offset;
    }
}
