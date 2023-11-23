using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerFootsIK : MonoBehaviour
{
    [SerializeField] float _offset;

    [SerializeField] Animator _anim;
    [SerializeField] LayerMask _groundMask;

    [SerializeField] Transform _leftFootRayTrans;
    [SerializeField] Transform _rightFootRayTrans;

    [SerializeField] TwoBoneIKConstraint _RFIK;
    [SerializeField] TwoBoneIKConstraint _LFIK;

    [SerializeField] Transform _targetRightFoot;
    [SerializeField] Transform _targetLeftFoot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateIK()
    {
        RaycastHit hitL;
        RaycastHit hitR;
        Vector3 tmp;
        if (Physics.Raycast(_leftFootRayTrans.position, _leftFootRayTrans.forward, out hitL, 1f, _groundMask))
        {
            tmp = hitL.point;
            tmp.y += _offset;
            _targetLeftFoot.position = tmp;
            //_targetLeftFoot.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(_targetLeftFoot.forward, hitL.normal));
            _LFIK.weight = _anim.GetFloat("LF IK Weight");
        }
        if (Physics.Raycast(_rightFootRayTrans.position, _rightFootRayTrans.forward, out hitR, 1f, _groundMask))
        {
            tmp = hitR.point;
            tmp.y += _offset;
            _targetRightFoot.position = tmp;
            //_targetRightFoot.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(_targetRightFoot.forward, hitR.normal));
            _RFIK.weight = _anim.GetFloat("RF IK Weight");
        }
    }
}

