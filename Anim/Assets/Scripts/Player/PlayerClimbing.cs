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

    [SerializeField] CheckForWallContacts _checkForWall;
    [SerializeField] Camera _playerCamera;

    [SerializeField] Transform _player;
    [SerializeField] Rigidbody _playerRigidbody;
    [SerializeField] float _startClimbingSpeed;
    [SerializeField] float _checkLength;
    [SerializeField,Range(0,1)] float _wallCheckAcceptance;
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

    [SerializeField] TwoBoneIKConstraint _RHIKWallTouch;
    [SerializeField] Transform _targetRHWallTouch;

    [SerializeField] float _checkDistance;

    private Transform[] _limbsTransform;
    private TwoBoneIKConstraint[] _IKs;
    private Transform[] _targets;
    private Vector3[] _allHitNormals;
    [SerializeField] float[] _limbsOffsets = new float[4];
    private Quaternion[] _targetsOffset;

    private float angleAboutX;
    private float angleAboutZ;

    private float _touchWall;

    private bool[] _allGroundSphereCastHits;
    private RaycastHit hit;
    private float addedHeight = 0.6f;

    private Vector3 aveHitNormal;
    private bool _moveHandTowardsWall;

    private Vector3 _lineStart;
    private Vector3 _lineEnd;

    bool _isCycling = false;
    bool _isFirstCycle = true;


    Coroutine _climbingCor;

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

        _checkForWall.OnNearWall += EnableMoveHandTowardsWall;
        _checkForWall.OnLeftWall += DisableMovehandTowardswall;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void EnableMoveHandTowardsWall()=> _moveHandTowardsWall = true;
    private void DisableMovehandTowardswall() => _moveHandTowardsWall = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="movingDirection"></param>
    /// <returns>returns value betrween 0 to 1 which represents how far from wall hand is</returns>
    public float MoveHandTowardsWall(Vector2 movingDirection)
    {
        if (_moveHandTowardsWall)
        {
            Quaternion targetRot = Quaternion.identity;
            Quaternion camRot = Quaternion.identity;
            camRot.eulerAngles = new Vector3(0, _playerCamera.transform.rotation.eulerAngles.y, 0);
            targetRot.eulerAngles = new Vector3(0, MathF.Atan2(movingDirection.y, -movingDirection.x) * (180 / Mathf.PI) - 90, 0);
            targetRot *= camRot;

            Ray ray = new Ray(_bodyCheck.position, _bodyCheck.forward);
            if (Physics.Raycast(ray, out hit, _checkLength, _climbingMask))
            {
                _targetRHWallTouch.position = hit.point;
                if (movingDirection == Vector2.zero) _touchWall -= Time.deltaTime * 4f;
                else
                {
                    if (Quaternion.Dot(targetRot, _player.rotation) >= _wallCheckAcceptance)
                    {
                        _touchWall += Time.deltaTime * 2f;
                    }
                    else _touchWall -= Time.deltaTime * 4f;
                }

            }
        }
        else _touchWall -= Time.deltaTime * 4f;
        _touchWall = math.clamp(_touchWall, 0, 1);
        _RHIKWallTouch.weight = _touchWall;
        return _touchWall;
    }
    public void RotateTowardsWall()
    {
        float maxXAnlge = 50f;

        float maxRotationStep = 1f;

        float averageHitNormalX = 0f;
        float averageHitNormalY = 0f;
        float averageHitNormalZ = 0f;

        for(int i=0;i<4;i++)
        {
            averageHitNormalX += _allHitNormals[i].x;
            averageHitNormalY += _allHitNormals[i].y;
            averageHitNormalZ += _allHitNormals[i].z;
        }
        aveHitNormal = new Vector3(averageHitNormalX / 4, averageHitNormalY / 4, averageHitNormalZ / 4).normalized;
        _lineEnd = aveHitNormal;
        ProjectedAxisAngles(out angleAboutX,out angleAboutZ,_player,aveHitNormal);


        float characterXRotation = _player.transform.eulerAngles.x;
        float characterZRotation = _player.transform.eulerAngles.z;

        if (characterXRotation > 180f) characterXRotation -= 360;
        if (characterZRotation > 180f) characterZRotation -= 360;

        //if (characterXRotation + angleAboutX < -maxXAnlge) angleAboutX = maxXAnlge + characterXRotation;
        //else if (characterXRotation+angleAboutX>maxXAnlge) angleAboutX = maxXAnlge - characterXRotation;
        //angleAboutX += 90f;
        float bodyEulerX = Mathf.MoveTowardsAngle(0, angleAboutX, maxRotationStep);
        float bodyEulerZ = Mathf.MoveTowardsAngle(0, angleAboutZ, maxRotationStep);
        Debug.Log(angleAboutX + " " + bodyEulerX);
        _player.eulerAngles = new Vector3(_player.eulerAngles.x + bodyEulerX, _player.eulerAngles.y, _player.eulerAngles.z);

    }
    public void StopClimbing()
    {
        StopCoroutine(_climbingCor);
        _playerRigidbody.useGravity = true;
        _playerRig.weight = 0;
    }
    public void SetUpLimbs()
    {
        RotateCharacterLimb();
        //RotateTowardsWall();
    }

    public bool CheckForClimbFromAir(Vector2 movingDirection)
    {
        Quaternion targetRot = Quaternion.identity;
        Quaternion camRot = Quaternion.identity;
        camRot.eulerAngles = new Vector3(0, _playerCamera.transform.rotation.eulerAngles.y, 0);
        targetRot.eulerAngles = new Vector3(0, MathF.Atan2(movingDirection.y, -movingDirection.x) * (180 / Mathf.PI) - 90, 0);
        targetRot *= camRot;

        Ray ray = new Ray(_bodyCheck.position, _bodyCheck.forward);
        if (Physics.Raycast(ray, out hit, _checkLength, _climbingMask))
        {
            if (movingDirection == Vector2.zero) return false;
            else
            {
                if (Quaternion.Dot(targetRot, _player.rotation) >= _wallCheckAcceptance) return true;
                else return false;
            }

        }
        return false;


    }
    public void StartClimbingFromAir()
    {
        _playerRigidbody.useGravity = false;
        _playerRigidbody.velocity = Vector3.zero;
        _playerRig.weight = 1;
        OnStartClimbing?.Invoke();

    }
    public void MoveToStartClimbingPos()
    {

        _climbingCor = StartCoroutine(StartClimbingCor());
        
    }
    public Vector3 ProjectOnContactPlane(Vector3 vector,Vector3 hitNormal)
    {
        return vector - hitNormal * Vector3.Dot(vector, hitNormal); //Vector3.ProjectOnPlane(vector, hitNormal);
    }
    void ProjectedAxisAngles(out float angleAboutX, out float angleAboutZ, Transform limbTargetTransform, Vector3 hitNormal)
    {
        Vector3 xAxisProjected = ProjectOnContactPlane(limbTargetTransform.forward,hitNormal).normalized;
        Vector3 zAxisProjected = ProjectOnContactPlane(limbTargetTransform.right, hitNormal).normalized;
        Vector3 yAxisProjected = ProjectOnContactPlane(limbTargetTransform.up,hitNormal).normalized;

        angleAboutX = Vector3.SignedAngle(limbTargetTransform.forward, xAxisProjected, limbTargetTransform.right);
        angleAboutZ = Vector3.SignedAngle(limbTargetTransform.right, zAxisProjected, limbTargetTransform.forward);
    }

    private void RotateCharacterLimb()
    {
        for(int i=0;i<4;i++)
        {
            CastRayForLimb(_limbsTransform[i], addedHeight, out Vector3 hitPoint, out _, out Vector3 hitNormal, out _allGroundSphereCastHits[i]);
            _allHitNormals[i] = hitNormal;
            //if (i == 0) Debug.Log(hitNormal);
            if (_allGroundSphereCastHits[i] == true)
            {
                ProjectedAxisAngles(out angleAboutX, out angleAboutZ, _limbsTransform[i], _allHitNormals[i]);

                _targets[i].position = hitPoint; // maybe add offset (target is in ankle)
                //_targets[i].rotation = _limbsTransform[i].rotation;
                //_targets[i].rotation *= _targetsOffset[i];
                //if (i == 0) Debug.Log(angleAboutX +" "+angleAboutZ);
                //_targets[i].localEulerAngles = new Vector3(_targets[i].localEulerAngles.x + angleAboutX, _targets[i].localEulerAngles.y, _targets[i].localEulerAngles.z + angleAboutZ);
            }
            else
            {
                _targets[i].position = _limbsTransform[i].position;
                //_targets[i].rotation = _limbsTransform[i].rotation;
                //_targets[i].rotation *= _targetsOffset[i];
            }
        }
    }

    IEnumerator StartClimbingCor()
    {
        _playerRigidbody.useGravity = false;
        float value = 0;
        Vector3 startClimbingPos = _startClimbingTran.position;
        Vector3 pos = transform.position;
        while (value < 1f)
        {
            transform.position = new Vector3(transform.position.x, math.lerp(pos.y, startClimbingPos.y, value), startClimbingPos.z);
            value += Time.deltaTime * _startClimbingSpeed;
            yield return null;
        }
        OnStartClimbing?.Invoke();
        _RHIKWallTouch.weight = 0;
        _playerRig.weight = 1;
    }
    private void CastRayForLimb(Transform origin,float addedHeight ,out Vector3 hitpoint,out float currentHitDistance,out Vector3 hitNormal,out bool gotCastHit)
    {
        Vector3 startRay = origin.position - (_player.rotation*new Vector3(0, 0, addedHeight));
        Ray limbRay = new Ray(startRay, _player.forward);
        RaycastHit hit;
        if (Physics.Raycast(limbRay, out hit, _checkDistance, _climbingMask))
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(_bodyCheck.position, _bodyCheck.position + _bodyCheck.forward * _checkLength);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Vector3.zero, _lineEnd*10f);
    }
}
