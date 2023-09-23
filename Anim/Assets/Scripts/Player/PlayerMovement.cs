using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum MoveState 
    {
        WALK,RUN,FAST_RUN
    }

    [SerializeField] float _rotationSpeed = 5f;
    [SerializeField] float _combatMovementSpeed;
    [SerializeField] float _combatBackMovementSpeed;
    [SerializeField] float _runSpeed;
    [SerializeField] float _walkSpeed;
    [SerializeField] float _fastRunSpeed;
    [SerializeField] float _backMoveSpeed;
    [SerializeField] Rigidbody _rb;
    [SerializeField] Ringhandle _jumphandle;
    [SerializeField] Camera _cam;
    [SerializeField] float _jumpForce;
    [SerializeField] Transform _playerBody;
    private float _rotationAngle;
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
        Debug.Log("ADD for");
        _rb.velocity = Vector3.zero;
        if (isMoving) _rb.AddForce(_jumphandle.GetVector() * _jumpForce);
        else _rb.AddForce(Vector3.up * _jumpForce/2); ;

    }
    public void RotatePlayerBack()
    {
        _playerBody.Rotate(Vector3.up, -_rotationAngle);
        _rotationAngle = 0;
    }
    public void Roll(Vector2 direction)
    {
        _rotationAngle =(MathF.Atan2(direction.y, -direction.x) * 180 / Mathf.PI)-90;
       // if (math.abs(_rotationAngle) == 90) _rotationAngle *= -1;
        //Debug.Log("x: "+direction.x+" y: "+direction.y +" "+_rotationAngle);
        _playerBody.Rotate(Vector3.up, _rotationAngle);
    }
    public void LandOnGround()
    {
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
    }
    public void Move(Vector2 direction, MoveState moveState)
    {
        if(direction!=Vector2.zero)
        {
            Quaternion targetRot = Quaternion.identity;
            Quaternion camRot = Quaternion.identity;
            camRot.eulerAngles = new Vector3(0, _cam.transform.rotation.eulerAngles.y, 0);
            targetRot.eulerAngles = new Vector3(0, MathF.Atan2(direction.y, -direction.x) * (180 / Mathf.PI) - 90, 0);
            targetRot *= camRot;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * _rotationSpeed);
            //_rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * _rotationSpeed));
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRot, Time.deltaTime * _rotationSpeed);
        }
        float speed = 0;
        switch(moveState)
        {
            case MoveState.WALK: speed = _walkSpeed;break;
            case MoveState.RUN: speed = _runSpeed;break;
            case MoveState.FAST_RUN: speed = _fastRunSpeed;break;
        }

        float value = 0;



        if (direction.x != 0 || direction.y != 0) value = 1;
        _rb.velocity = transform.forward * speed*value;// new Vector3( direction.x*speed, 0, direction.y*speed);
       // transform.Translate(Vector3.forward * value * Time.deltaTime * speed);
    }
    private void LateUpdate()
    {

    }
}
