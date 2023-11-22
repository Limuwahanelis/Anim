using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeDetection : MonoBehaviour
{
    [SerializeField] Transform _mainSlopeDetectorTrans;
    [SerializeField] Transform _frontSlopeDetectorTrans;
    [SerializeField] float _mainSlopeDetectionRayLength;
    [SerializeField] float _frontSlopeDetectionRayLength;
    [SerializeField] LayerMask _groundMask;
    private Vector3 _floorNormal;
    public bool IsNearGround => _isNearGround;
    private bool _isNearGround;
    private Vector3 _rayStart;
    private Vector3 _rayDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SlopeDetect();
    }
    public void SetGroundMask(LayerMask mask)
    {
        _groundMask = mask;
    }
    public float SlopeDetect()
    {
        RaycastHit hit;
        RaycastHit hitFront;
        float angle = 0;
        if (Physics.Raycast(_mainSlopeDetectorTrans.position, _mainSlopeDetectorTrans.forward, out hit, _mainSlopeDetectionRayLength, _groundMask))
        {
            //Debug.DrawRay(hit.point, hit.normal, Color.green);
            angle = Vector3.SignedAngle(transform.up, hit.normal, transform.right);
            Vector3 cross = Vector3.Cross(hit.normal, transform.up).normalized;
            //Debug.DrawRay(transform.position, cross, Color.blue);
            float dot = Vector3.Dot(cross, transform.forward);
            //Debug.Log("dot: "+dot);
            //Debug.Log("raw angle: " + angle);
            //Debug.Log(string.Format("{0} % from {1} = {2}",dot>=0?( 1 - dot):(1+dot), angle, (dot >= 0 ? (1 - dot) : (1 + dot)) * angle));   
            angle = (dot >= 0 ? (1 - dot) : (1 + dot)) * angle;
            
            _isNearGround = true;
            _floorNormal = hit.normal;
            
        }
        else _isNearGround = false;
        if (Physics.Raycast(_frontSlopeDetectorTrans.position, _frontSlopeDetectorTrans.forward, out hitFront, _frontSlopeDetectionRayLength, _groundMask))
        {
            angle = Vector3.SignedAngle(transform.up, hitFront.normal, transform.right);
            Vector3 cross = Vector3.Cross(hitFront.normal, transform.up).normalized;
            float dot = Vector3.Dot(cross, transform.forward);
           angle = (dot >= 0 ? (1 - dot) : (1 + dot)) * angle;
            
            _floorNormal = hitFront.normal;
        }
        Debug.Log(angle);
        return angle;
    }
    public Vector3 GetFloorNormal()
    {
        return _floorNormal;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_mainSlopeDetectorTrans.position, _mainSlopeDetectorTrans.position+ _mainSlopeDetectorTrans.forward * _mainSlopeDetectionRayLength);
        Gizmos.DrawLine(_frontSlopeDetectorTrans.position, _frontSlopeDetectorTrans.position + _frontSlopeDetectorTrans.forward * _frontSlopeDetectionRayLength);
        //Gizmos.color
        //Gizmos.DrawRay(transform.position+new Vector3(0,1.5f,0),)
    }

}
