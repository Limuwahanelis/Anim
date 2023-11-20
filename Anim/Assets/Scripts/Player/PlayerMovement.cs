using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    public enum MoveState 
    {
        WALK,RUN,FAST_RUN
    }

    public Action OnEndedClimb;
    public Vector3 PlayerPosition=> transform.position;
    [SerializeField] float _climbSpeed=2f;
    public float ClimbSpeed => _climbSpeed;
    [SerializeField] float _rotationSpeed = 5f;
    [SerializeField] float _combatMovementSpeed;
    [SerializeField] float _combatBackMovementSpeed;
    [SerializeField] float _runSpeed;
    [SerializeField] float _walkSpeed;
    [SerializeField] float _fastRunSpeed;
    [SerializeField] float _backMoveSpeed;
    [SerializeField] Rigidbody _rb;
    public Rigidbody PlayerRB => _rb;
    [SerializeField] Ringhandle _jumphandle;
    [SerializeField] Camera _cam;
    [SerializeField] float _jumpForce;
    [SerializeField] Transform _playerController;
    [SerializeField] Transform _playerBody;
    public Transform PlayerBody => _playerBody;
    private float _rotationAngle;

    private bool _isRotating;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Jump(bool isMoving)
    {
        _rb.velocity = Vector3.zero;
        if (isMoving) _rb.AddForce(_jumphandle.GetVector() * _jumpForce);
        else _rb.AddForce(Vector3.up * _jumpForce/2); ;

    }
    public void RotatePlayerBack()
    {
        _playerController.Rotate(Vector3.up, -_rotationAngle);
        _rotationAngle = 0;
    }
    public void Roll(Vector2 direction)
    {
        _rotationAngle =(MathF.Atan2(direction.y, -direction.x) * 180 / Mathf.PI)-90;
       // if (math.abs(_rotationAngle) == 90) _rotationAngle *= -1;
        //Debug.Log("x: "+direction.x+" y: "+direction.y +" "+_rotationAngle);
        _playerController.Rotate(Vector3.up, _rotationAngle);
    }
    public void LandOnGround()
    {
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
    }

    public void Move(Vector2 direction, MoveState moveState)
    {
        bool canMove=true;
        if(direction!=Vector2.zero)
        {
            //Quaternion targetRot = Quaternion.identity;
            //Quaternion camRot = Quaternion.identity;
            //camRot.eulerAngles = new Vector3(0, _cam.transform.rotation.eulerAngles.y, 0);
            //targetRot.eulerAngles = new Vector3(0, MathF.Atan2(direction.y, -direction.x) * (180 / Mathf.PI) - 90, 0);
            //targetRot *= camRot;
            //_rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRot, Time.deltaTime * _rotationSpeed);
            canMove = !Rotate(direction);
        }
        float speed = 0;
        switch(moveState)
        {
            case MoveState.WALK: speed = _walkSpeed;break;
            case MoveState.RUN: speed = _runSpeed;break;
            case MoveState.FAST_RUN: speed = _fastRunSpeed;break;
        }

        float value = 0;


        if (canMove)
        {
            if (direction.x != 0 || direction.y != 0) value = 1;
            Vector3 moveVector = transform.forward * speed * value;
            moveVector.y = _rb.velocity.y;
            _rb.velocity = moveVector;// new Vector3( direction.x*speed, 0, direction.y*speed);
                                                             // transform.Translate(Vector3.forward * value * Time.deltaTime * speed);
        }
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        //_rb.MovePosition(position);
    }
    IEnumerator RotationCor(Vector2 direction)
    {
        if(_isRotating) yield break;
        _isRotating = true;
        Quaternion targetRot = Quaternion.identity;
        Quaternion camRot = Quaternion.identity;
        camRot.eulerAngles = new Vector3(0, _cam.transform.rotation.eulerAngles.y, 0);
        targetRot.eulerAngles = new Vector3(0, MathF.Atan2(direction.y, -direction.x) * (180 / Mathf.PI) - 90, 0);
        targetRot *= camRot;
        while (Quaternion.Dot(_rb.rotation, targetRot) < 0.98)
        {
            Debug.Log("Rota");

            
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRot, Time.deltaTime * _rotationSpeed);
            yield return null;
        }
        _isRotating = false;
    }
    private bool Rotate(Vector2 direction)
    {
        Quaternion targetRot = Quaternion.identity;
        Quaternion camRot = Quaternion.identity;
        camRot.eulerAngles = new Vector3(0, _cam.transform.rotation.eulerAngles.y, 0);
        targetRot.eulerAngles = new Vector3(0, MathF.Atan2(direction.y, -direction.x) * (180 / Mathf.PI) - 90, 0);
        targetRot *= camRot;
        _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRot, Time.deltaTime * _rotationSpeed);
        if (Quaternion.Dot(_rb.rotation, targetRot) < 0.98) return true;
        else return false;
    }
    private void LateUpdate()
    {

    }
}
