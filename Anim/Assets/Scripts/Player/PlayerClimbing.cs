using RPGCharacterAnims.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

// TO DO calculate distance between target and limb and move targt if its too far
// TO DO adjust ik at corners

public class PlayerClimbing : MonoBehaviour
{
    public Action OnStartClimbing;
    public Action<Vector3> OnFoundFloor;
    public List<Collider> Walls => _checkForWall.Walls;

    [Header("Objects")]

    [SerializeField] PlayerClimbingAnimatorController _animationController;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerVaulting _playerVaulting;
    [SerializeField] CheckForWallContacts _checkForWall;
    [SerializeField] Camera _playerCamera;
    [SerializeField] PlayerChecks _playerChecks;
    [SerializeField] LayerMask _climbingMask;
    [SerializeField] Rig _playerRig;

    [Header("Transforms")]
    [SerializeField] Transform _player;
    [SerializeField] Transform _spineTarget;
    [SerializeField] Transform _bodyCheck;
    [SerializeField] Transform _startClimbingTran;
    [SerializeField] Transform _targetRHWallTouch;

    [SerializeField] Transform _targetRightHand;
    [SerializeField] Transform _targetLeftHand;
    [SerializeField] Transform _targetRightLeg;
    [SerializeField] Transform _targetLeftLeg;

    [SerializeField] Transform _RHTransform;
    [SerializeField] Transform _LHTransform;
    [SerializeField] Transform _RLTransform;
    [SerializeField] Transform _LLTransform;

    [SerializeField] Transform _RHForwardTrans;
    [SerializeField] Transform _LHForwardTrans;
    [SerializeField] Transform _RLForwardTrans;
    [SerializeField] Transform _LLForwardTrans;

    [Header("Constraints")]

    [SerializeField] TwoBoneIKConstraint _RHIK;
    [SerializeField] TwoBoneIKConstraint _LHIK;
    [SerializeField] TwoBoneIKConstraint _RLIK;
    [SerializeField] TwoBoneIKConstraint _LLIK;
    [SerializeField] TwoBoneIKConstraint _RHIKWallTouch;

    [Header("Variables")]

    [SerializeField] float _startClimbingSpeed;
    [SerializeField] float _checkLength;
    [SerializeField,Range(0,1)] float _wallCheckAcceptance;
    [SerializeField] float _climbSpeed;
    [SerializeField] float _checkDistance;
    [SerializeField] float[] _limbsOffsets = new float[4];

    private Transform[] _limbsTransform;
    private Transform[] _targets;
    private Transform[] _limbsForwardTran;
    private Vector3[] _allHitNormals;
    
    private float _touchWall;
    private bool[] _allGroundSphereCastHits;
    private RaycastHit hit;
    private float addedHeight = 0.3f;
    private bool _moveHandTowardsWall;
    Coroutine _climbingCor;

    //////////////////////////////////////
    [SerializeField] float _rayTowardsWall = 1f;
    [SerializeField] float _headHelperOffsetFromFeet = 1.6f;
    [SerializeField] float _spineHelperOffsetFromFeet = 1f;
    [SerializeField] float _rayTowardsMoveDir = 1;
    [SerializeField] float _rotateSpeed = 2f;
    [SerializeField] float _positionOffset;
    [SerializeField] float _offsetFromWall;
    Quaternion _previousRotation;
    Transform _headHelper;
    Transform _helper;
    Transform _spineHelper;
    Vector3 _targetPos;
    Vector3 _startPos;
    bool _isLerping;
    float t;
    Vector2 _lastClimbingDirection = Vector2.zero;

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

        _limbsForwardTran = new Transform[4];
        _limbsForwardTran[0] = _RHForwardTrans;
        _limbsForwardTran[1] = _LHForwardTrans;
        _limbsForwardTran[2] = _RLForwardTrans;
        _limbsForwardTran[3] = _LLForwardTrans;


        _allGroundSphereCastHits = new bool[5];
        _allHitNormals = new Vector3[4];

        _checkForWall.OnNearWall += EnableMoveHandTowardsWall;
        _checkForWall.OnLeftWall += DisableMovehandTowardswall;
    }
    void InitHelper()
    {
        if (_helper != null)
        {
            _helper.position = transform.position;
            _helper.rotation = transform.rotation;
            _headHelper.rotation = _helper.rotation;
            _spineHelper.rotation = _helper.rotation;
            _spineHelper.position = _headHelper.position;
            _headHelper.position = _helper.position+ _helper.up * _headHelperOffsetFromFeet;
            return;
        }
        _helper = new GameObject().transform;
        _spineHelper = new GameObject().transform;
        _headHelper = new GameObject().transform;
        _helper.name = "climb_helper";
        _spineHelper.name = "climb_spine_helper";
        _headHelper.name = "climb_head_helper";
        _helper.position = transform.position;
        _helper.rotation = transform.rotation;
        _headHelper.position = _helper.position + _helper.up * _headHelperOffsetFromFeet;
        _spineHelper.position = _headHelper.position;
        _spineHelper.rotation = _helper.rotation;
    }

    public void Climb(Vector2 direction)
    {
        if (Vector2.Dot(direction, _lastClimbingDirection) == -1)
        {
            Vector3 tmp = _startPos;
            _startPos = _helper.position;
            _helper.position = tmp;
            _helper.rotation = _previousRotation;
            _targetPos = _helper.position;
            _lastClimbingDirection = direction;
            t = 1 - t;
        }
        else if (_lastClimbingDirection != direction && direction!=Vector2.zero)
        {
            _isLerping = false;
            
        }
        if (!_isLerping)
        {
            Vector3 h = _helper.right * direction.x;
            Vector3 v = _helper.up * direction.y;
            Vector3 moveDir = (h + v).normalized;
            if (direction == Vector2.zero) return;
            bool canMove = CanMove(moveDir);
            if (!canMove)  return;
            _headHelper.position = _helper.position + _helper.up * _headHelperOffsetFromFeet;
            _headHelper.rotation = _helper.rotation;
            _spineHelper.position = _helper.position + _helper.up * _spineHelperOffsetFromFeet;
            _spineHelper.rotation = _helper.rotation;
            RaycastHit hit;
            if(Physics.Raycast(_spineHelper.position,_spineHelper.forward,out hit,3f,_climbingMask))
            {
                _spineHelper.rotation = Quaternion.LookRotation(-hit.normal);
            }
            _lastClimbingDirection = direction;
            t = 0;
            _isLerping = true;
            _previousRotation = _helper.rotation;
            _startPos = transform.position;
            _targetPos = _helper.position;
        }
        else
        {
            t += Time.deltaTime * _climbSpeed;
            if (t > 1)
            {
                t = 1;
                _isLerping = false;
            }
            Vector3 cp = Vector3.Lerp(_startPos, _targetPos, t);

            Debug.DrawRay(_player.position - _player.forward, _player.forward * 3f);
            _playerMovement.PlayerRB.MovePosition(cp);
            _playerMovement.PlayerRB.rotation = Quaternion.Slerp(_playerMovement.PlayerRB.rotation, _helper.rotation, Time.deltaTime * _rotateSpeed);
            _spineTarget.rotation = Quaternion.Slerp(_spineTarget.rotation, _spineHelper.rotation, Time.deltaTime);
        }
    }
    bool CanMove(Vector3 moveDir)
    {
        Vector3 origin = transform.position;

        float dis = _rayTowardsMoveDir;
        Vector3 dir = moveDir;
        Debug.DrawRay(origin, dir * dis, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(origin, dir, out hit, dis,_climbingMask))// checks if there is wall perpendicualar to us
        {
            //if (Vector3.Dot(Vector3.up, hit.normal) >= 0.95 && hit.collider.gameObject.layer == )
            //{
            //    Debug.Log("Floor");
            //    return false;
            //}
            _helper.position = PosWithOffset(origin, hit.point);
            _helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }





        Debug.DrawRay(_headHelper.position, _headHelper.forward*0.5f, Color.blue);
        RaycastHit headHelperHit;
        if(Physics.Raycast(_headHelper.position,_headHelper.forward,out headHelperHit,0.5f,_climbingMask))
        {
            float angle = Vector3.SignedAngle(headHelperHit.normal, Vector3.up, Vector3.Cross(headHelperHit.normal, Vector3.up));
            Debug.Log(angle);
            if (angle < 46f)
            {

                Debug.Log("should vault");
                StopClimbing();
                OnFoundFloor?.Invoke(headHelperHit.point);
                return false;
            }
        }

        Vector3 headHelperOrigin = _headHelper.position + _headHelper.forward * 0.5f;
        Debug.DrawRay(headHelperOrigin, Vector3.down * 1f, Color.cyan);
        if (Physics.Raycast(headHelperOrigin, Vector3.down, out headHelperHit, 1f, _climbingMask))
        {
            float angle = Vector3.SignedAngle(headHelperHit.normal, Vector3.up, Vector3.Cross(headHelperHit.normal, Vector3.up));
            Debug.Log(angle);
            if (angle < 46f)
            {
                Debug.Log("should vault cor");
                StopClimbing();
                OnFoundFloor?.Invoke(headHelperHit.point);
                return false;
            }

        }

        origin += moveDir * dis;

        dir = _helper.forward;
        float dis2 = _rayTowardsWall;
        //raycast forwards towards the wall
        Debug.DrawRay(origin, dir * dis2, Color.blue);
        if (Physics.Raycast(origin, dir, out hit, dis2, _climbingMask))
        {
            //Debug.Log(Vector3.SignedAngle(hit.normal, Vector3.up,Vector3.Cross(hit.normal,Vector3.up)));
            _helper.position = PosWithOffset(origin, hit.point);
            _helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }


        //origin = origin + (dir * dis2);
        origin = origin + (dir * dis);
        dir = -moveDir;
        Debug.DrawRay(origin, dir , Color.black);
        // raycast for around corners 
        if (Physics.Raycast(origin, dir, out hit, _rayTowardsWall))
        {
            Debug.Log("coee");
            _helper.position = PosWithOffset(origin, hit.point);
            _helper.rotation = Quaternion.LookRotation(-hit.normal);
            return true;
        }




        origin += dir * dis2;
        dir = -Vector3.up;

        Debug.DrawRay(origin, dir, Color.yellow);
        if (Physics.Raycast(origin, dir, out hit, dis2, _climbingMask))
        {
            if(Vector3.Dot(Vector3.up, hit.normal) >= 0.95)
            {
                RaycastHit hit2;
                if (!Physics.Raycast(origin, Vector3.up, out hit2, 2, _climbingMask))
                 {
                    Debug.Log("vault");
                }
            }
            float angle = Vector3.Angle(_helper.up, hit.normal);
            if (angle < 40)
            {
                _helper.position = PosWithOffset(origin, hit.point);
                _helper.rotation = Quaternion.LookRotation(-hit.normal);
                return true;
            }
        }

        return false;
    }
    Vector3 PosWithOffset(Vector3 origin, Vector3 target)
    {
        Vector3 direction = origin - target;
        direction.Normalize();
        Vector3 offset = direction * _offsetFromWall;
        return target + offset;
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
    public void StopClimbing()
    {
        if (_climbingCor != null)
        {
            StopCoroutine(_climbingCor);
            _climbingCor = null;
            t = 0;
            _isLerping = false;
        }
        _playerMovement.PlayerRB.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        _lastClimbingDirection = Vector2.zero;
        _playerMovement.PlayerRB.isKinematic = false;
        _playerRig.weight = 0;
        _touchWall = 0;
    }
    public void SetUpLimbs()
    {
        if (_isLerping) RotateCharacterLimb();

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
        _playerMovement.PlayerRB.useGravity = false;
        _playerMovement.PlayerRB.velocity = Vector3.zero;
        _playerRig.weight = 1;
        InitHelper();
        OnStartClimbing?.Invoke();

    }
    public void MoveToStartClimbingPos()
    {
        _climbingCor = StartCoroutine(StartClimbingCor());
    }
    private void RotateCharacterLimb()
    {
        for(int i=0;i<4;i++)
        {
            CastRayForLimb(_limbsTransform[i], addedHeight, _limbsForwardTran[i], out Vector3 hitPoint, out _, out Vector3 hitNormal, out _allGroundSphereCastHits[i]);
            _allHitNormals[i] = hitNormal;
            if (_allGroundSphereCastHits[i] == true)
            {

                _targets[i].position = hitPoint;// maybe add offset (target is in ankle)
                Vector3 tmp = _targets[i].position;
                tmp -= _limbsForwardTran[i].forward * _limbsOffsets[i];
                //tmp.z-= _limbsOffsets[i]; ;
                _targets[i].position = tmp;
                _targets[i].rotation = Quaternion.LookRotation(-_allHitNormals[i]);
            }
            else
            {
                _targets[i].position = _limbsTransform[i].position;
            }
        }
    }

    private void Vault()
    {

    }
    IEnumerator StartClimbingCor()
    {
        
        _playerMovement.PlayerRB.useGravity = false;
        _playerMovement.PlayerRB.isKinematic = true;
        float value = 0;
        Vector3 startClimbingPos = _startClimbingTran.position;
        Vector3 pos = transform.position;
        Vector3 newPos = PosWithOffset(pos, startClimbingPos); //transform.position;
        Debug.Log(startClimbingPos+" "+ newPos);
        newPos.x = pos.x;
        while (value < 1f)
        {

            newPos.y = math.lerp(pos.y, startClimbingPos.y, value);
            _playerMovement.PlayerRB.Move(newPos, _playerMovement.PlayerRB.rotation);
            
            value += Time.deltaTime * _startClimbingSpeed;
            yield return null;
        }
        Debug.Log(newPos);
        OnStartClimbing?.Invoke();
        _RHIKWallTouch.weight = 0;
        _playerRig.weight = 1;
        InitHelper();
    }
    private void CastRayForLimb(Transform origin, float addedHeight, Transform limbForwardTran, out Vector3 hitpoint, out float currentHitDistance, out Vector3 hitNormal, out bool gotCastHit)
    {
        hitNormal = Vector3.zero;
        gotCastHit = false;
        hitpoint = origin.position;
        currentHitDistance = 0;
        //Vector3 startRay = origin.position - (_player.rotation*new Vector3(0, 0, addedHeight));
        //Vector3 startRay = origin.position - (limbForwardTran.rotation* new Vector3(0, 0, addedHeight));

        Vector3 startRay = origin.position - limbForwardTran.forward * addedHeight;
        Ray limbRay = new Ray(startRay, limbForwardTran.forward / 2);
        Debug.DrawRay(startRay, limbForwardTran.forward / 2, Color.red);
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
            startRay = origin.position + limbForwardTran.forward * 0.15f;
            if (CheckLimbRay(startRay, limbForwardTran.right, out hitpoint, out currentHitDistance, out hitNormal, out gotCastHit)) return;
            if (CheckLimbRay(startRay, -limbForwardTran.right, out hitpoint, out currentHitDistance, out hitNormal, out gotCastHit)) return;
            if (CheckLimbRay(startRay, limbForwardTran.up, out hitpoint, out currentHitDistance, out hitNormal, out gotCastHit)) return;
            if (CheckLimbRay(startRay, -limbForwardTran.up, out hitpoint, out currentHitDistance, out hitNormal, out gotCastHit)) return;
            else
            {
                hitNormal = Vector3.zero;
                gotCastHit = false;
                hitpoint = origin.position;
                currentHitDistance = 0;
            }
        }
    }
    private bool CheckLimbRay(Vector3 rayStart,Vector3 rayDirection,out Vector3 hitpoint, out float currentHitDistance, out Vector3 hitNormal, out bool gotCastHit)
    {
        hitNormal = Vector3.zero;
        gotCastHit = false;
        hitpoint = rayStart;
        currentHitDistance = 0;
        Ray limbRay = new Ray(rayStart, rayDirection);
        Debug.DrawRay(rayStart, rayDirection, Color.green);
        if (Physics.Raycast(limbRay, out hit, _checkDistance, _climbingMask))
        {
            hitNormal = hit.normal;
            hitpoint = hit.point;
            currentHitDistance = hit.distance + addedHeight;
            gotCastHit = true;
            return true;
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(_bodyCheck.position, _bodyCheck.position + _bodyCheck.forward * _checkLength);
    }
    ///////////////////
    


}
