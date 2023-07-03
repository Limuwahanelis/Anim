using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecks : MonoBehaviour
{
    [SerializeField] Transform _groundCheckPos;
    [SerializeField] Vector3 _groundCheckHalfExtents;
    [SerializeField] LayerMask _groundMask;
    public bool IsTouchingGround => isTouchingGround;
    private bool isTouchingGround;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround=Physics.CheckBox(_groundCheckPos.position, _groundCheckHalfExtents,Quaternion.identity, _groundMask);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_groundCheckPos.position, _groundCheckHalfExtents*2);
    }
}
