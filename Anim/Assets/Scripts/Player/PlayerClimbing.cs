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

    [SerializeField] Transform _targetRightHand;
    [SerializeField] Transform _targetLeftHand;
    [SerializeField] Transform _targetRightLeg;
    [SerializeField] Transform _targetLeftLeg;

    [SerializeField] Transform _RHTransform;
    [SerializeField] Transform _LHTransform;
    [SerializeField] Transform _RLTransform;
    [SerializeField] Transform _LLTransform;

    [SerializeField] TwoBoneIKConstraint _RHIK;
    [SerializeField] TwoBoneIKConstraint _LHIK;
    [SerializeField] TwoBoneIKConstraint _RLIK;
    [SerializeField] TwoBoneIKConstraint _LLIK;

    private Transform[] _limbsTransform;
    private TwoBoneIKConstraint[] _IKs;
    private Transform[] _targets;
    private Vector3[] _allHitNormals;
    private Quaternion[] _targetsOffset;

    private float angleAboutX;
    private float angleAboutZ;

    private bool[] _allGroundSphereCastHits;
    private bool _isClimbing = false;
    private RaycastHit hit;
    private float addedHeight = 0.6f;

    bool _isCycling = false;
    bool _isFirstCycle = true;


    // Start is called before the first frame update
    void Start()
    {
        _targets = new Transform[4];
        _targets[0] = _targetRightHand;
        _targets[1] = _targetLeftHand;
        _targets[2] = _targetRightLeg;
        _targets[3] = _targetLeftLeg;

        _limbsTransform = new Transform[4];
        _limbsTransform[0] = _RHTransform;
        _limbsTransform[1] = _LHTransform;
        _limbsTransform[2] = _RLTransform;
        _limbsTransform[3] = _LLTransform;

        _IKs = new TwoBoneIKConstraint[4];
        _IKs[0] = _RHIK;
        _IKs[1] = _LHIK;
        _IKs[2] = _RLIK;
        _IKs[3] = _LLIK;

        _targetsOffset = new Quaternion[4];
        _targetsOffset[0] = _targets[0].rotation;
        _targetsOffset[1] = _targets[1].rotation;
        _targetsOffset[2] = _targets[2].rotation;
        _targetsOffset[3] = _targets[3].rotation;

        _allGroundSphereCastHits = new bool[5];
        _allHitNormals = new Vector3[4];
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
    public void SetUpLimbs()
    {
        RotateCharacterLimb();
        //Vector3 hitpoint;
        //float currentHitDistance;
        //CastRayForLimb(_RHTransform,addedHeight, out hitpoint,out currentHitDistance,out _,out _);
        //_targetRightHand.position = hitpoint;
        //CastRayForLimb(_LHTransform, addedHeight, out hitpoint, out currentHitDistance, out _, out _);
        //_targetLeftHand.position = hitpoint;
        //CastRayForLimb(_RLTransform, addedHeight, out hitpoint, out currentHitDistance, out _, out _);
        //_targetRightLeg.position = hitpoint;
        //CastRayForLimb(_LLTransform, addedHeight, out hitpoint, out currentHitDistance, out _, out _);
        //_targetLeftLeg.position = hitpoint;
    }
    public void MoveToStartClimbingPos()
    {
        
        StartCoroutine(StartClimbingCor());
    }
    public Vector3 ProjectOnContactPlane(Vector3 vector,Vector3 hitNormal)
    {
        return vector - hitNormal * Vector3.Dot(vector, hitNormal); //Vector3.ProjectOnPlane(vector, hitNormal);
    }
    void ProjectedAxisAngles(out float angleAboutX, out float angleAboutZ, Transform limbTargetTransform, Vector3 hitNormal)
    {
        Vector3 xAxisProjected = ProjectOnContactPlane(limbTargetTransform.forward,hitNormal).normalized;
        Vector3 zAxisProjected = ProjectOnContactPlane(limbTargetTransform.right, hitNormal).normalized;

        angleAboutX = Vector3.SignedAngle(limbTargetTransform.forward, xAxisProjected, limbTargetTransform.right);
        angleAboutZ = Vector3.SignedAngle(limbTargetTransform.right, zAxisProjected, limbTargetTransform.forward);
    }

    private void RotateCharacterLimb()
    {
        for(int i=0;i<4;i++)
        {
            CastRayForLimb(_limbsTransform[i], addedHeight, out Vector3 hitPoint, out _, out Vector3 hitNormal, out _allGroundSphereCastHits[i]);
            _allHitNormals[i] = hitNormal;

            if (_allGroundSphereCastHits[i] == true)
            {
                ProjectedAxisAngles(out angleAboutX, out angleAboutZ, _limbsTransform[i], _allHitNormals[i]);

                _targets[i].position = hitPoint; // maybe add offset (target is in ankle)
                _targets[i].rotation = _limbsTransform[i].rotation;
                //_targets[i].rotation *= _targetsOffset[i];
                _targets[i].localEulerAngles = new Vector3(_targets[i].localEulerAngles.x + angleAboutX, _targets[i].localEulerAngles.y, _targets[i].localEulerAngles.z + angleAboutZ);
            }
            else
            {
                _targets[i].position = _limbsTransform[i].position;
                _targets[i].rotation = _limbsTransform[i].rotation;
                //_targets[i].rotation *= _targetsOffset[i];
            }
        }
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
        OnStartClimbing?.Invoke();
        _playerRig.weight = 1;
    }
    private void CastRayForLimb(Transform origin,float addedHeight ,out Vector3 hitpoint,out float currentHitDistance,out Vector3 hitNormal,out bool gotCastHit)
    {
        hitpoint = Vector3.zero;
        Vector3 startRay = origin.position - (_player.rotation*new Vector3(0, 0, addedHeight));
        Ray limbRay = new Ray(startRay, _player.forward);
        RaycastHit hit;
        if (Physics.Raycast(limbRay, out hit, 20f, _climbingMask))
        {
            hitNormal = hit.normal;
            hitpoint = hit.point;
            currentHitDistance = hit.distance + addedHeight;
            gotCastHit = true;
        }
        else
        {
            hitNormal = Vector3.zero;
            gotCastHit = false;
            hitpoint = origin.position;
            currentHitDistance = 0;
        }
    }
    public void Cycle(float speed)
    {
        if (_isCycling) return;
    }
    public IEnumerator ClimbCycleCor(float speed)
    {
        if(_isCycling) yield break;
        _isCycling = true;
        Debug.Log("Start cycle;");
        Vector3[] hitPositions = new Vector3[4];
        Vector3[] snapPositions = new Vector3[4];

        for(int i=0;i<4;i++)
        {
            snapPositions[i] = _targets[i].position;
        }
        float value = 0f;
        while(value<1f)
        {
            for(int i=0;i<4;i++)
            {
                 _targets[i].position= Vector3.Lerp(snapPositions[i], hitPositions[i], value);
            }
            value += Time.deltaTime * speed;
            yield return null;
        }
        _isCycling = false;
        _isFirstCycle = !_isFirstCycle;
        Debug.Log("End cycle;");
    }
}
