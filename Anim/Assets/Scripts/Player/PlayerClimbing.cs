using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerClimbing : MonoBehaviour
{
    public Action OnStartClimbing;
    [SerializeField] Transform _player;
    [SerializeField] Rigidbody _playerRigidbody;
    [SerializeField] float _startClimbingSpeed;
    [SerializeField] float _checkLength;
    [SerializeField] float _climbSpeed;
    [SerializeField] Transform _overHeadCheck;
    [SerializeField] Transform _headCheck;
    [SerializeField] Transform _bodyCheck;
    [SerializeField] LayerMask _climbingMask;
    [SerializeField] Transform _startClimbingTran;
    [SerializeField] Rig _playerRig;
    [SerializeField] Transform _baseRightHand;
    [SerializeField] Transform _baseLeftHand;
    [SerializeField] Transform _baseRightLeg;
    [SerializeField] Transform _baseLeftLeg;

    [SerializeField] Transform _base2RightHand;
    [SerializeField] Transform _base2LeftHand;
    [SerializeField] Transform _base2RightLeg;
    [SerializeField] Transform _base2LeftLeg;

    [SerializeField] Transform _targetRightHand;
    [SerializeField] Transform _targetLeftHand;
    [SerializeField] Transform _targetRightLeg;
    [SerializeField] Transform _targetLeftLeg;
    private bool _isClimbing = false;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray climbRay = new Ray(_bodyCheck.position,_bodyCheck.forward);
        RaycastHit hit;
        if(Physics.Raycast(climbRay,out hit, _checkLength, _climbingMask))
        {
            if(!_isClimbing)
            {
                Quaternion rot=Quaternion.identity;
                _playerRigidbody.isKinematic = true;
                rot.SetFromToRotation(_player.forward, -hit.normal);
                _playerRigidbody.MoveRotation(rot);
                OnStartClimbing?.Invoke();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(_bodyCheck.position,_bodyCheck.position+_bodyCheck.forward* _checkLength);
    }

    public void MoveToStartClimbingPos()
    {
        _targetLeftHand.position = _baseLeftHand.position;
        _targetLeftHand.rotation = _baseLeftHand.rotation;
        _targetRightHand.position = _baseRightHand.position;
        _targetRightHand.rotation = _baseRightHand.rotation;
        _targetLeftLeg.position = _baseLeftLeg.position;
        _targetLeftLeg.rotation = _baseLeftLeg.rotation;
        _targetRightLeg.position = _baseRightLeg.position;
        _targetRightLeg.rotation = _baseRightLeg.rotation;
        _playerRig.weight = 1;
        StartCoroutine(StartClimbingCor());
    }
    public void RotatePlaeyrToBeParalelToWall()
    {
       
    }
    IEnumerator StartClimbingCor()
    {
        _isClimbing = true;
        float value = 0;
        Vector3 startClimbingPos = _startClimbingTran.position;
        Vector3 pos = transform.position;
        while (value < 1f)
        {
            transform.position = new Vector3(transform.position.x, math.lerp(pos.y, startClimbingPos.y, value), transform.position.z);
            value += Time.deltaTime * _startClimbingSpeed;
            yield return null;
        }
        StartCoroutine(ClimbCor());
    }

    IEnumerator ClimbCor()
    {
        bool cylce1 = true;
        float value=0f;
        while(true)
        {
            if(cylce1)
            {
                _targetRightHand.position = Vector3.Lerp(_baseRightHand.position, _base2RightHand.position, value);
                value+= Time.deltaTime;
                if (value > 1f)
                {
                    value = 0f;
                    cylce1 = false;
                }
            }
            else
            {
                _targetRightHand.position = Vector3.Lerp(_base2RightHand.position,_baseRightHand.position,value);
                value += Time.deltaTime;
                if (value > 1f)
                {
                    value = 0f;
                    cylce1 = true;
                }
            }
            yield return null;
        }
    }
}
