using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerClimbingState : PlayerState
{
    bool _isMoving;
    float _distance = 2f;
    float animSpeedZ = 0;
    float animSpeedX = 0;
    float _stoppingSpeedZ = 8f;
    float _accelerationSpeedZ = 3f;
    float _stoppingSpeedX = 8f;
    float _accelerationSpeedX = 3f;
    float _horizontalClimb = 0f;
    float _horizontalClimbAcceleration = 3f;
    int _cycleX = 1;
    int _lastCycle = 1;
    Vector2 _lastDirection = Vector2.zero;
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
        _lastCycle = _cycleX;
        if (direction.x != 0)
        {
            if (direction.x < 0)
            {
                animSpeedX -= _accelerationSpeedX * Time.deltaTime;
                animSpeedX = math.clamp(animSpeedX, -1, 1);
            }
            else
            {
                animSpeedX += _accelerationSpeedX * Time.deltaTime;
                animSpeedX = math.clamp(animSpeedX, -1, 1);
            }
            if(math.abs(animSpeedX)>=1)
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
        if (direction.y != 0)
        {
            if (direction.y < 0)
            {
                animSpeedZ -= _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, -1, 1);
            }
            else
            {
                animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, -1, 1);
            }
        }
        if (direction != Vector2.zero) _lastDirection = direction;
        else ChangeAnimationAccordingToLastDirection();
        _context.anim.SetFloat("SpeedZ", animSpeedZ);
        _context.anim.SetFloat("SpeedX", animSpeedX);
        _context.anim.SetFloat("Horizontal_Climb", _horizontalClimb);
    }

    public override void Drop()
    {
        _context.playerClimbing.StopClimbing();
        _context.ChangePlayerState(new PlayerStopClimbingState(_context));
    }
    private void ChangeAnimationAccordingToLastDirection()
    {
        if (math.abs(_lastDirection.x) > 0)
        {
            if (_lastDirection.x < 0)
            {

                animSpeedX -= _accelerationSpeedX * Time.deltaTime;
                animSpeedX = math.clamp(animSpeedX, -1, 1);
                _horizontalClimb +=_accelerationSpeedX * Time.deltaTime;
                _horizontalClimb = math.clamp(_horizontalClimb, 1, _cycleX);
            }
            else
            {
                animSpeedX += _accelerationSpeedX * Time.deltaTime;
                animSpeedX = math.clamp(animSpeedX, -1, 1);
                _horizontalClimb += _accelerationSpeedX * Time.deltaTime;
                _horizontalClimb = math.clamp(_horizontalClimb, 1, _cycleX);
            }

            if (animSpeedZ > 0)
            {
                animSpeedZ -= _stoppingSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 1);
            }
            else
            {
                animSpeedZ += _stoppingSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, -1, 0);
            }

        }
        else if (math.abs(_lastDirection.y) > 0)
        {
            if (_lastDirection.y < 0)
            {
                animSpeedZ -= _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, -1, 1);
            }
            else
            {
                animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, -1, 1);
            }

            if (animSpeedX > 0)
            {
                animSpeedX -= _stoppingSpeedX * Time.deltaTime;
                animSpeedX = math.clamp(animSpeedX, 0, 1);
            }
            else
            {
                animSpeedX += _stoppingSpeedX * Time.deltaTime;
                animSpeedX = math.clamp(animSpeedX, -1, 0);
            }
        }

    }
    private IEnumerator ClimbCor(Vector2 direction)
    {
        if (_isMoving) yield break;
        Debug.Log("start");
        _isMoving = true;
        float _traveledDistance = 0f;
        while(_traveledDistance < _distance)
        {
            _context.playerMovement.Climb(direction);
            _traveledDistance+=_context.playerMovement.ClimbSpeed*Time.deltaTime;
            yield return null;
        }
        Debug.Log("end");
        _isMoving =false;
    }
}