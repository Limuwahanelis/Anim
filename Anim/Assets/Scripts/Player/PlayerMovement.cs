using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 5f;
    [SerializeField] float _speed;
    [SerializeField] float _backMoveSpeed;
    [SerializeField] Rigidbody _rb;
    [SerializeField] Camera _cam;
    [SerializeField] float _jumpForce;
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
    public void LandOnGround()
    {
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
    }
    public void Move(Vector2 direction)
    {
        if (direction.y >= 0) transform.Translate(Vector3.forward * direction.y*Time.deltaTime*_speed + Vector3.right * direction.x * Time.deltaTime * _speed);// _rb.velocity = new Vector3( direction.x * _speed, _rb.velocity.y, direction.y * _speed);
        else  transform.Translate(Vector3.forward * direction.y * Time.deltaTime*_backMoveSpeed + Vector3.right * direction.x * Time.deltaTime * _backMoveSpeed);// new Vector3(direction.x * _backMoveSpeed, _rb.velocity.y, direction.y * _backMoveSpeed);
    }
    private void LateUpdate()
    {
        float angle = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, _cam.transform.rotation.eulerAngles.y);
        //Quaternion.Lerp(transform._rotation, _cam.transform._rotation, Time.deltaTime * _speed);
       // _rb.MoveRotation(Quaternion.Lerp(transform._rotation, _cam.transform._rotation, Time.deltaTime * _speed));
         transform.Rotate(Vector3.up, angle * Time.deltaTime * _rotationSpeed);
    }
}
