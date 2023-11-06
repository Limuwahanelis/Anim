using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerClimbingAnimatorController : MonoBehaviour
{
    [SerializeField] Animator _anim;
    bool _isMoving;
    float _distance = 2f;
    float _stoppingSpeedZ = 4f;
    float _accelerationSpeedZ = 3f;
    float _stoppingSpeedX = 4f;
    float _accelerationSpeedX = 3f;
    float _horizontalClimb = 0f;
    float _horizontalClimbAcceleration = 1.5f;
    float _verticalClimb = 0f;
    float _verticalClimbAcceleration = 1.5f;
    float _diagonalClimbL = 0f;
    float _diagonalClimbR = 0f;
    int _cycleX = 1;
    int _cycleY = 1;
    int _cycleDL = 1;
    int _cycleDR = 1;
    bool _isBothPressed = true;
    Vector2 _currentDirection = Vector2.zero;
    Vector2 _lastDirection = Vector2.zero;
    Vector2 _normalClimb = Vector2.zero;
    Vector2 _moveVector = Vector2.zero;
    float testX = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void aa(Vector2 lastClimbDirection, Vector2 direction, float delta)
    {
        _isBothPressed = true;

        if (lastClimbDirection == Vector2.zero && delta == 1)
        {

        }
        if (lastClimbDirection.x == 0)
        {
            if (_normalClimb.x > 0)
            {
                _normalClimb.x -= _stoppingSpeedX * Time.deltaTime;
                _normalClimb.x = math.clamp(_normalClimb.x, 0, 1);
            }
            else
            {
                _normalClimb.x += _stoppingSpeedX * Time.deltaTime;
                _normalClimb.x = math.clamp(_normalClimb.x, -1, 0);
            }
        }
        else
        {
            if (lastClimbDirection.x < 0)
            {
                if (_normalClimb.x < 0)
                {
                    if (_normalClimb.x <= -1)
                    {
                        _horizontalClimb = _cycleX - 1 + delta;
                        _cycleX = (int)math.floor(_horizontalClimb + 1);
                        if (_cycleX >= 3) _cycleX = 1;
                    }
                    else
                    {
                        _normalClimb.x = -2 * (delta - 0.5f);
                        if (_horizontalClimb < _cycleX) _horizontalClimb += _horizontalClimbAcceleration * Time.deltaTime;
                    }
                }
                else
                {
                    _normalClimb.x = 1 - 2 * delta;
                    if (_horizontalClimb < _cycleX) _horizontalClimb += Time.deltaTime * _horizontalClimbAcceleration;
                }
            }
            else
            {
                if (_normalClimb.x < 0)
                {
                    _normalClimb.x = -(1 - 2 * delta);
                }
                else
                {
                    if (_normalClimb.x >= 1)
                    {
                        _horizontalClimb = _cycleX - 1 + delta;
                        _cycleX = (int)math.floor(_horizontalClimb + 1);
                        if (_cycleX >= 3) _cycleX = 1;
                    }
                    else
                    {
                        _normalClimb.x = 2 * (delta - 0.5f);
                        if (_horizontalClimb < _cycleX) _horizontalClimb += Time.deltaTime * _horizontalClimbAcceleration;
                    }
                }
            }
        }
        if (lastClimbDirection.y == 0)
        {

            if (_normalClimb.y > 0)
            {
                _normalClimb.y -= _stoppingSpeedZ * Time.deltaTime;
                _normalClimb.y = math.clamp(_normalClimb.y, 0, 1);
            }
            else
            {
                _normalClimb.y += _stoppingSpeedZ * Time.deltaTime;
                _normalClimb.y = math.clamp(_normalClimb.y, -1, 0);
            }

        }
        else
        {
            if (lastClimbDirection.y < 0)
            {
                if (_normalClimb.y < 0)
                {
                    _normalClimb.y = -2 * (delta - 0.5f);
                }
                else
                {
                    _normalClimb.y = 1 - 2 * delta;
                }
            }
            else
            {
                if (_normalClimb.y < 0)
                {
                    _normalClimb.y = -(1 - 2 * delta);
                }
                else
                {
                    _normalClimb.y = 2 * (delta - 0.5f);
                }
            }
        }
        //if (math.abs(_normalClimb.x) >= 1)
        //{
        //    if (_lastDirection.x * lastClimbDirection.x > 0)
        //    {
        //        _horizontalClimb = 1 + delta * 2;
        //        //if (_horizontalClimb >= 3) _horizontalClimb = 1;
        //        //_horizontalClimb = math.clamp(_horizontalClimb, 1, 3);
        //        //_cycleX = (int)math.ceil(_horizontalClimb);
        //    }
        //}






        //if (math.abs(_normalClimb.y) >= 1)
        //{
        //    if (_lastDirection.y * lastClimbDirection.y > 0)
        //    {
        //        _verticalClimb += _verticalClimbAcceleration * Time.deltaTime;
        //        if (_verticalClimb > 3) _verticalClimb = 1;
        //        _verticalClimb = math.clamp(_verticalClimb, 1, 3);
        //        _cycleY = (int)math.ceil(_verticalClimb);
        //    }
        //}
        //else _isBothPressed = false;


        //if (_isBothPressed)
        //{
        //    if (Vector2.SqrMagnitude(_normalClimb) >= 2)
        //    {
        //        if (_normalClimb.x * _normalClimb.y == 1)
        //        {
        //            _diagonalClimbR += math.sqrt(math.pow(_verticalClimbAcceleration * Time.deltaTime, 2) + math.pow(_horizontalClimbAcceleration * Time.deltaTime, 2));
        //            if (_diagonalClimbR > 3) _diagonalClimbR = 1;
        //            _diagonalClimbR = math.clamp(_diagonalClimbR, 1, 3);
        //            _cycleDR = (int)math.ceil(_diagonalClimbR);
        //        }
        //        else
        //        {
        //            _diagonalClimbL += math.sqrt(math.pow(_verticalClimbAcceleration * Time.deltaTime, 2) + math.pow(_horizontalClimbAcceleration * Time.deltaTime, 2));
        //            if (_diagonalClimbL > 3) _diagonalClimbL = 1;
        //            _diagonalClimbL = math.clamp(_diagonalClimbL, 1, 3);
        //            _cycleDL = (int)math.ceil(_diagonalClimbL);
        //        }
        //    }
        //}

        //if (lastClimbDirection != _lastDirection)
        //{
        //    //_context.playerClimbing.Climb(lastClimbDirection);
        //    _lastDirection = lastClimbDirection;
        //    ChangeAnimationAccordingToLastDirection();
        //    //Debug.Log(_lastDirection);
        //}
        // else ChangeAnimationAccordingToLastDirection();

        //_horizontalClimb += _horizontalClimbAcceleration * Time.deltaTime;
        //_horizontalClimb = math.clamp(_horizontalClimb, 1, _cycleX);
        _anim.SetFloat("SpeedZ", _normalClimb.y);
        _anim.SetFloat("SpeedX", _normalClimb.x);
        _anim.SetFloat("Horizontal_Climb", _horizontalClimb);
        _anim.SetFloat("Vertical_Climb", _verticalClimb);
        _anim.SetFloat("Diagonal_ClimbR", _diagonalClimbR);
        _anim.SetFloat("Diagonal_ClimbL", _diagonalClimbL);
    }

    public void ChangeAnimationAccordingToLastDirection()
    {
        bool any = false;
        _moveVector = Vector2.zero;

    }
}
