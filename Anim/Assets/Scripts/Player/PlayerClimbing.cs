using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.Rendering.DebugUI.Table;

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

    private Transform[] targets;
    private bool _isClimbing = false;
    private RaycastHit hit;

    bool _isCycling = false;
    bool _isFirstCycle = true;
    // Start is called before the first frame update
    void Start()
    {
        targets = new Transform[4];
        targets[0] = _targetRightHand;
        targets[1] = _targetLeftHand;
        targets[2] = _targetRightLeg;
        targets[3] = _targetLeftLeg;
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
    public void RotateTowardsWall()
    {
        Ray climbRay = new Ray(_bodyCheck.position, _bodyCheck.forward);
        RaycastHit hit;
        if (Physics.Raycast(climbRay, out hit, _checkLength, _climbingMask))
        {
                Quaternion rot = Quaternion.identity;
                _playerRigidbody.isKinematic = true;
                rot.SetFromToRotation(_player.forward, -hit.normal);
                _playerRigidbody.MoveRotation(rot);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(_bodyCheck.position,_bodyCheck.position+_bodyCheck.forward* _checkLength);
    }

    public void MoveToStartClimbingPos()
    {
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
        //StartCoroutine(ClimbCor());
        Quaternion rot = Quaternion.identity;
        Ray limbRay = new Ray(_base2RightHand.position, _base2RightHand.forward);
        RaycastHit hit;
        if (Physics.Raycast(limbRay, out hit, 20f, _climbingMask))
        {
            _targetRightHand.position = hit.point;
            rot = Quaternion.FromToRotation(_targetRightHand.up, hit.normal);
            _targetRightHand.rotation = rot;
        }
        //CastRayForLeftHand();

        Vector3 hitpoint;
        CastRayForLimb(_base2LeftHand,out hitpoint);
        _targetLeftHand.position = hitpoint;
        CastRayForLimb(_base2RightHand,out hitpoint);
        _targetRightHand.position = hitpoint;
        CastRayForLimb(_base2LeftLeg, out hitpoint);
        _targetLeftLeg.position = hitpoint;
        CastRayForLimb(_base2RightLeg, out hitpoint);
        _targetRightLeg.position = hitpoint;
        OnStartClimbing?.Invoke();
    }
    private void CastRayForLimb(Transform origin, out Vector3 hitpoint)
    {
        hitpoint = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        Ray limbRay = new Ray(origin.position, _base2LeftHand.forward);
        RaycastHit hit;
        if (Physics.Raycast(limbRay, out hit, 20f, _climbingMask))
        {
            hitpoint = hit.point;
        }
    }
    public void Cycle(float speed)
    {
        if (_isCycling) return;
        StartCoroutine(ClimbCycleCor(speed, _isFirstCycle));
    }
    public IEnumerator ClimbCycleCor(float speed ,bool firstCycle)
    {
        if(_isCycling) yield break;
        _isCycling = true;
        Debug.Log("Start cycle;");
        Vector3[] hitPositions = new Vector3[4];
        Vector3[] snapPositions = new Vector3[4];

        for(int i=0;i<4;i++)
        {
            snapPositions[i] = targets[i].position;
        }
        if (firstCycle)
        {
            CastRayForLimb(_baseRightHand, out hitPositions[0]);
            CastRayForLimb(_baseLeftHand, out hitPositions[1]);
            CastRayForLimb(_baseRightLeg, out hitPositions[2]);
            CastRayForLimb(_baseLeftLeg, out hitPositions[3]);
        }
        else
        {
            CastRayForLimb(_base2RightHand, out hitPositions[0]);
            CastRayForLimb(_base2LeftHand, out hitPositions[1]);
            CastRayForLimb(_base2RightLeg, out hitPositions[2]);
            CastRayForLimb(_base2LeftLeg, out hitPositions[3]);
        }
        float value = 0f;
        while(value<1f)
        {
            for(int i=0;i<4;i++)
            {
                 targets[i].position= Vector3.Lerp(snapPositions[i], hitPositions[i], value);
            }
            value += Time.deltaTime * speed;
            yield return null;
        }
        _isCycling = false;
        _isFirstCycle = !_isFirstCycle;
        Debug.Log("End cycle;");
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_base2LeftHand.position, _base2LeftHand.position + _base2LeftHand.forward * 3f);
    }
}
