using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecks : MonoBehaviour
{
    [SerializeField] Transform _groundCheckPos;
    [SerializeField] Vector3 _groundCheckHalfExtents;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] SlopeDetection _slopeDetection;
    public bool IsTouchingGround => isTouchingGround;
    public float FloorAngle;
    private bool isTouchingGround;
    // Start is called before the first frame update
    void Start()
    {
        _slopeDetection.SetGroundMask(_groundMask);
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround=Physics.CheckBox(_groundCheckPos.position, _groundCheckHalfExtents,Quaternion.identity, _groundMask);
        FloorAngle=_slopeDetection.SlopeDetect();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_groundCheckPos.position, _groundCheckHalfExtents*2);
    }

}
