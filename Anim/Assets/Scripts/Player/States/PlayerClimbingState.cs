using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerClimbingState : PlayerState
{
    bool _isMoving;
    float _distance = 2f;
    float _stoppingSpeedZ = 8f;
    float _accelerationSpeedZ = 3f;
    float _stoppingSpeedX = 8f;
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
    int _lastCycle = 1;
    bool _isBothPressed = true;
    Vector2 _lastDirection = Vector2.zero;
    Vector2 _normalClimb = Vector2.zero;
    Vector2 _diagonalDirection = Vector2.zero;
    Vector2 _moveVector = Vector2.zero;

    public PlayerClimbingState(PlayerContext context) : base(context)
    {

    }

    public override void Update()
    {
        

    }
    public override void FixedUpdate()
    {
        _context.playerClimbing.SetUpLimbs();
    }
    public override void SetUpState()
    {
        
    }

    public override void InterruptState()
    {
     
    }
    public override void Move(Vector2 direction)
    {
        //_context.playerClimbing.Climb(direction);
        _isBothPressed = true;
        _lastCycle = _cycleX;
        if (direction.x != 0)
        {
            if (direction.x < 0)
            {
                _normalClimb.x -= _accelerationSpeedX * Time.deltaTime;
                _normalClimb.x = math.clamp(_normalClimb.x, -1, 1);
            }
            else
            {
                _normalClimb.x += _accelerationSpeedX * Time.deltaTime;
                _normalClimb.x = math.clamp(_normalClimb.x, -1, 1);
            }

            if (direction.y == 0)
            {

                if (_normalClimb.y > 0)
                {
                    _normalClimb.y -= _accelerationSpeedZ * Time.deltaTime;
                    _normalClimb.y = math.clamp(_normalClimb.y, 0, 1);
                }
                else
                {
                    _normalClimb.y += _accelerationSpeedZ * Time.deltaTime;
                    _normalClimb.y = math.clamp(_normalClimb.y, -1, 0);
                }

            }
            if (math.abs(_normalClimb.x) >= 1)
            {
                if (_lastDirection.x * direction.x > 0)
                {
                    _horizontalClimb += _horizontalClimbAcceleration * Time.deltaTime;
                    if (_horizontalClimb > 3) _horizontalClimb = 1;
                    _horizontalClimb = math.clamp(_horizontalClimb, 1, 3);
                    _cycleX = (int)math.ceil(_horizontalClimb);
                }
            }
        }
        else _isBothPressed = false;
        if (direction.y != 0)
        {
            if (direction.y < 0)
            {
                _normalClimb.y -= _accelerationSpeedZ * Time.deltaTime;
                _normalClimb.y = math.clamp(_normalClimb.y, -1, 1);
            }
            else
            {
                _normalClimb.y += _accelerationSpeedZ * Time.deltaTime;
                _normalClimb.y = math.clamp(_normalClimb.y, -1, 1);
            }


            if (direction.x == 0)
            {
                if (_normalClimb.x > 0)
                {
                    _normalClimb.x -= _accelerationSpeedX * Time.deltaTime;
                    _normalClimb.x = math.clamp(_normalClimb.x, 0, 1);
                }
                else
                {
                    _normalClimb.x += _accelerationSpeedX * Time.deltaTime;
                    _normalClimb.x = math.clamp(_normalClimb.x, -1, 0);
                }
            }

            if (math.abs(_normalClimb.y) >= 1)
            {
                if (_lastDirection.y * direction.y > 0)
                {
                    _verticalClimb += _verticalClimbAcceleration * Time.deltaTime;
                    if (_verticalClimb > 3) _verticalClimb = 1;
                    _verticalClimb = math.clamp(_verticalClimb, 1, 3);
                    _cycleY = (int)math.ceil(_verticalClimb);
                }
            }
        }
        else _isBothPressed = false;


        if (_isBothPressed)
        {
            if (Vector2.SqrMagnitude(_normalClimb) >= 2)
            {
                if (_normalClimb.x * _normalClimb.y == 1)
                {
                    _diagonalClimbR += math.sqrt(math.pow(_verticalClimbAcceleration * Time.deltaTime, 2) + math.pow(_horizontalClimbAcceleration * Time.deltaTime, 2));
                    if (_diagonalClimbR > 3) _diagonalClimbR = 1;
                    _diagonalClimbR = math.clamp(_diagonalClimbR, 1, 3);
                    _cycleDR = (int)math.ceil(_diagonalClimbR);
                }
                else
                {
                    _diagonalClimbL += math.sqrt(math.pow(_verticalClimbAcceleration * Time.deltaTime, 2) + math.pow(_horizontalClimbAcceleration * Time.deltaTime, 2));
                    if (_diagonalClimbL > 3) _diagonalClimbL = 1;
                    _diagonalClimbL = math.clamp(_diagonalClimbL, 1, 3);
                    _cycleDL = (int)math.ceil(_diagonalClimbL);
                }
            }
        }

        if (direction != Vector2.zero)
        {
            _context.playerClimbing.Climb(direction);
            _lastDirection = direction;
            //Debug.Log(_lastDirection);
        }
        else ChangeAnimationAccordingToLastDirection();
        //_context.playerClimbing.RotateTowardsWall();
        _context.anim.SetFloat("SpeedZ", _normalClimb.y);
        _context.anim.SetFloat("SpeedX", _normalClimb.x);
        _context.anim.SetFloat("Horizontal_Climb", _horizontalClimb);
        _context.anim.SetFloat("Vertical_Climb", _verticalClimb);
        _context.anim.SetFloat("Diagonal_ClimbR", _diagonalClimbR);
        _context.anim.SetFloat("Diagonal_ClimbL", _diagonalClimbL);
    }

    public override void Drop()
    {
        _context.playerClimbing.StopClimbing();
        _context.ChangePlayerState(new PlayerStopClimbingState(_context));
    }
    private void ChangeAnimationAccordingToLastDirection()
    {
        bool any = false;
        _moveVector = Vector2.zero;
        if (_lastDirection.x * _lastDirection.y != 0)
        {
            if (_normalClimb.x * _normalClimb.y == 1)
            {
                _diagonalClimbR += math.sqrt(math.pow(_verticalClimbAcceleration * Time.deltaTime, 2) + math.pow(_horizontalClimbAcceleration * Time.deltaTime, 2));
                _diagonalClimbR = math.clamp(_diagonalClimbR, 1, _cycleDR);
                if (_diagonalClimbR < _cycleDR) any = true;
            }
            else
            {
                _diagonalClimbL += math.sqrt(math.pow(_verticalClimbAcceleration * Time.deltaTime, 2) + math.pow(_horizontalClimbAcceleration * Time.deltaTime, 2));
                _diagonalClimbL = math.clamp(_diagonalClimbL, 1, _cycleDL);
                if (_diagonalClimbL<_cycleDL) any = true;
            }
        }
        if (math.abs(_lastDirection.x) > 0)
        {
            
            _moveVector.x = _lastDirection.x;
           // _moveVector.y = 0;
            if (math.abs(_normalClimb.x) < 1)
            {

                if (_lastDirection.x < 0)
                {
                    _normalClimb.x -= _accelerationSpeedX * Time.deltaTime;
                    _normalClimb.x = math.clamp(_normalClimb.x, -1, 1);
                }
                else
                {
                    _normalClimb.x += _accelerationSpeedX * Time.deltaTime;
                    _normalClimb.x = math.clamp(_normalClimb.x, -1, 1);
                }
                any = true;
                // _context.playerClimbing.Climb(_moveVector);
            }
            _horizontalClimb += _horizontalClimbAcceleration * Time.deltaTime;
            _horizontalClimb = math.clamp(_horizontalClimb, 1, _cycleX);

            if (_horizontalClimb < _cycleX) any = true;
            if (_lastDirection.y == 0)
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

        }
        if (math.abs(_lastDirection.y) > 0)
        {
            
            //_moveVector.x = 0;
            _moveVector.y = _lastDirection.y;
            if (math.abs(_normalClimb.y) < 1)
            {

                if (_lastDirection.y < 0)
                {
                    _normalClimb.y -= _accelerationSpeedZ * Time.deltaTime;
                    _normalClimb.y = math.clamp(_normalClimb.y, -1, 1);

                }
                else
                {
                    _normalClimb.y += _accelerationSpeedZ * Time.deltaTime;
                    _normalClimb.y = math.clamp(_normalClimb.y, -1, 1);
                }
                any = true;
                //_context.playerClimbing.Climb(_moveVector);

            }

            _verticalClimb += _verticalClimbAcceleration * Time.deltaTime;
            _verticalClimb = math.clamp(_verticalClimb, 1, _cycleY);

            if (_verticalClimb < _cycleY) any = true;
            if (_lastDirection.x == 0)
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

        }
       if(any) _context.playerClimbing.Climb(_moveVector);
    }
    private IEnumerator ClimbCor(Vector2 direction)
    {
        if (_isMoving) yield break;
        Debug.Log("start");
        _isMoving = true;
        float _traveledDistance = 0f;
        while(_traveledDistance < _distance)
        {
            _context.playerClimbing.Climb(direction);
            _traveledDistance+=_context.playerMovement.ClimbSpeed*Time.deltaTime;
            yield return null;
        }
        Debug.Log("end");
        _isMoving =false;
    }
}