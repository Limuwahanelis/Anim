using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 5f;
    [SerializeField] float _combatMovementSpeed;
    [SerializeField] float _combatBackMovementSpeed;
    [SerializeField] float _speed;
    [SerializeField] float _backMoveSpeed;
    [SerializeField] Rigidbody _rb;
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
    public void Jump()
    {
        Debug.Log("ADD for");
        _rb.useGravity = true;
        _rb.AddForce(Vector3.up*_jumpForce);

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
    public void Move(Vector2 direction, bool isInCombat)
    {
        if (direction.y >= 0) transform.Translate(Vector3.forward * direction.y*Time.deltaTime*(isInCombat?_combatMovementSpeed:_speed) + Vector3.right * direction.x * Time.deltaTime * (isInCombat ? _combatMovementSpeed : _speed));// _rb.velocity = new Vector3( direction.x * _speed, _rb.velocity.y, direction.y * _speed);
        else  transform.Translate(Vector3.forward * direction.y * Time.deltaTime*(isInCombat?_combatBackMovementSpeed: (isInCombat ? _combatBackMovementSpeed : _backMoveSpeed)) + Vector3.right * direction.x * Time.deltaTime * _backMoveSpeed);// new Vector3(direction.x * _backMoveSpeed, _rb.velocity.y, direction.y * _backMoveSpeed);
    }
    private void LateUpdate()
    {
        float angle = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, _cam.transform.rotation.eulerAngles.y);
        transform.Rotate(Vector3.up, angle * Time.deltaTime * _rotationSpeed);
    }
}
