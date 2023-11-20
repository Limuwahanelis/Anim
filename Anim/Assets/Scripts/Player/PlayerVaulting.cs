using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVaulting : MonoBehaviour
{
    public float VaultSpeed => _vaultSpeed;
    [SerializeField] LayerMask _climbingMask;
    [SerializeField] float _headHelperOffsetFromFeet = 1.6f;
    [SerializeField] float _rayTowardsMoveDir = 1;
    [SerializeField] float _rotateSpeed = 2f;
    [SerializeField] float _offsetFromWall;
    [SerializeField] float _feetOffset;
    [SerializeField] float _playerRadius;
    [SerializeField] float _playerHeight;
    [SerializeField] float _maxObstacleVaultHeight;
    [SerializeField] float _vaultSpeed;

    [Header("Debug")]
    [SerializeField] bool _drawDebug;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool CheckVault (out Vector3 targetPos)
    {
        // Debug.Log(moveDir);
        Vector3 moveDir = transform.forward;
        targetPos = Vector3.zero;
        Vector3 origin = transform.position;
        origin.y = transform.position.y + _feetOffset;

        float dis = _rayTowardsMoveDir;
        Vector3 dir = moveDir;
        Debug.DrawRay(origin, dir * dis, Color.red);
        RaycastHit hit;
        RaycastHit hit2;
        if (Physics.Raycast(origin, dir, out hit, dis, _climbingMask))// checks if there is obstacle in front
        {
            origin += moveDir * dis;
            origin += transform.forward * _playerRadius;
            dir = Vector3.up;
            float dis2 = _maxObstacleVaultHeight;
            Debug.DrawRay(origin, dir * dis2, Color.blue);

            if (!Physics.Raycast(origin, dir, out _, dis2, _climbingMask)) // check for space over the obstacle
            {
                origin += dir * dis2;
                dir = Vector3.down;
                Debug.DrawRay(origin, dir * _playerHeight, Color.green);
                if (Physics.Raycast(origin, dir, out hit2, _playerHeight, _climbingMask))
                {
                    targetPos = hit2.point;
                    return true;
                }
            }
            else return false;
        }

        return false;
    }
    private void OnDrawGizmosSelected()
    {
        if(!_drawDebug) return;
        Vector3 targetPos = Vector3.zero;
        Vector3 origin = transform.position;
        origin.y =transform.position.y+ _feetOffset;

        float dis = _rayTowardsMoveDir;
        Vector3 dir = transform.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin+ dir * dis);
        RaycastHit hit;
        RaycastHit hit2;
        if (Physics.Raycast(origin, dir, out hit, dis, _climbingMask))// checks if there is obstacle in front
        {
            origin += transform.forward * dis;
            origin += transform.forward * _playerRadius;
            dir = Vector3.up;
            float dis2 = _maxObstacleVaultHeight;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(origin, origin+ dir * dis2);

            if (!Physics.Raycast(origin, dir, out _, dis2, _climbingMask)) // check for space over the obstacle
            {
                origin += dir * dis2;
                dir = Vector3.down;
                Gizmos.color = Color.green;
                Gizmos.DrawLine(origin, origin+ dir * _playerHeight);
                if (Physics.Raycast(origin, dir, out hit2, _playerHeight, _climbingMask))
                {
                    Debug.Log("Vault");
                    targetPos = hit2.point;
                }
            }
        }

    }
}
