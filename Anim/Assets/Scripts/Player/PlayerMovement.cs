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
        if(direction!=Vector2.zero)
        {
            Quaternion targetRot = Quaternion.identity;
            Quaternion camRot = Quaternion.identity;
            camRot.eulerAngles = new Vector3(0, _cam.transform.rotation.eulerAngles.y, 0);
            targetRot.eulerAngles = new Vector3(0, MathF.Atan2(direction.y, -direction.x) * (180 / Mathf.PI) - 90, 0);
            targetRot *= camRot;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * _rotationSpeed);
        }
        Debug.Log(direction);
        float value = 0;
        if (direction.x != 0 || direction.y != 0) value = 1;
        Debug.Log(value);
        transform.Translate(Vector3.forward * value * Time.deltaTime * (isInCombat ? _combatMovementSpeed : _speed));
    }
    private void LateUpdate()
    {

    }
}
