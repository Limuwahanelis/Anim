using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerFastRunState : PlayerState
{
    float staminaCost = 20f;
    float _stoppingSpeedZ = 3f;
    float _accelerationSpeedZ = 3f;
    float animSpeedZ = 0;
    Vector2 _previousDirection = Vector2.zero;
    bool _isMoving;
    public PlayerFastRunState(PlayerContext context) : base(context)
    {

    }

    public override void Update()
    {

    }

    public override void SetUpState()
    {
        _context.anim.SetTrigger("Fast_Run");
        _context.staminaBar.StopRegenerating();
    }
    public override void Move(Vector2 direction)
    {
        if(direction==Vector2.zero)
        {
            _context.anim.SetTrigger("Stop_Run");
            _context.anim.SetFloat("SpeedZ", 0);
            _context.ChangePlayerState(new NormalPlayerState(_context));
        }

        if (direction.x == 0 && direction.y == 0)
        {
            _isMoving = false;
            if (animSpeedZ > 0)
            {
                animSpeedZ -= _stoppingSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 2);
            }
        }
        else
        {
            _isMoving = true;
            if (math.abs(direction.x) > 0)
            {
                animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 2);
            }

            if (math.abs(direction.y) > 0)
            {
                animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 2);
            }
        }

        _context.playerMovement.Move(direction, PlayerMovement.MoveState.FAST_RUN);
        _context.staminaBar.IncreaseCurrentStamina(-staminaCost * Time.deltaTime);

        _context.anim.SetFloat("SpeedZ", animSpeedZ);
        _previousDirection = direction;
        if(_context.staminaBar.CurrentStamina<=0)
        {
            _context.anim.SetTrigger("Normal_Run");
            _context.ChangePlayerState(new NormalPlayerState(_context));
        }
    }
    public override void Jump()
    {
        _context.ChangePlayerState(new PlayerJumpingState(_context,_isMoving));
    }

    public override void InterruptState()
    {

    }
    public override void Attack()
    {
        _context.ChangePlayerState(new PlayerAttackingState(_context));
    }
    public override void ChangeMove()
    {
        _context.ChangePlayerState(new PlayerWalkingState(_context));

    }
}