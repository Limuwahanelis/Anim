using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepDetection : MonoBehaviour
{
    [SerializeField] Transform _stepDetectorTrans;
    [SerializeField] float _stepDetectionRayLength;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] float _feetOffset;
    [SerializeField] float _playerRadius;
    [SerializeField] float _playerHeight;
    [SerializeField] float _maxSmallStepHeight;
    [SerializeField] float _maxHighStepHeight=0.6f;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetGroundMask(LayerMask mask)
    {
        _groundMask = mask;
    }
    public bool DetectStep(out Vector3 targetPos)
    {
        targetPos = Vector3.zero;
        RaycastHit hitFront;
        if (Physics.Raycast(_stepDetectorTrans.position, _stepDetectorTrans.forward, out hitFront, _stepDetectionRayLength, _groundMask))
        {
            float angle = Vector3.SignedAngle(transform.up, hitFront.normal, transform.right);
            if (angle >= -90f && angle < -85f)
            {
                Vector3 moveDir = transform.forward;
                targetPos = Vector3.zero;
                Vector3 origin = transform.position;
                origin.y = transform.position.y + _feetOffset;

                float dis = _stepDetectionRayLength;
                Vector3 dir = moveDir;
                RaycastHit hit;
                RaycastHit hit2;

                origin += moveDir * dis;
                origin += transform.forward * _playerRadius;
                dir = Vector3.up;
                float dis2 = _playerHeight;

                if (!Physics.Raycast(origin, dir, out hit, dis2, _groundMask)) // check for space over the obstacle
                {
                    origin += dir * dis2;
                    dir = Vector3.down;
                    if (Physics.Raycast(origin, dir, out hit2, _playerHeight, _groundMask))
                    {
                        if (hit2.point.y - hitFront.point.y > _maxHighStepHeight) return false; // we should vault or climb
                        targetPos = hit2.point;
                        return true;
                    }
                }
                else return false;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_stepDetectorTrans.position, _stepDetectorTrans.position + _stepDetectorTrans.forward * _stepDetectionRayLength);
    }
}
