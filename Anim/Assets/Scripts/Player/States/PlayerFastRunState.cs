using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerFastRunState : PlayerState
{
    float staminaCost = 20f;
    float _stoppingSpeedZ = 3f;
    float _accelerationSpeedZ = 3f;
    float _minRunTime = 0.5f;
    float _runTime = 0f;
    float animSpeedZ = 0;
    Vector2 _previousDirection = Vector2.zero;
    bool _isMoving;
    public PlayerFastRunState() : base()
    {

    }


    public override void Update()
    {

    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
         animSpeedZ = 0;
         _previousDirection = Vector2.zero;
         _isMoving=false;
        _runTime = 0f;
        _context.anim.SetTrigger("Fast_Run");
        _context.staminaBar.StopRegenerating();
    }
    public override void Move(Vector2 direction)
    {
        if(direction==Vector2.zero && _runTime>=_minRunTime)
        {
            _context.anim.SetTrigger("Stop_Run");
            _context.anim.SetFloat("SpeedZ", 0);
            NormalPlayerState.SetAsCurrentState(_context.getState(typeof(NormalPlayerState)), _context);
            return;
        }

        if (direction.x == 0 && direction.y == 0)
        {
            _isMoving = false;
            if (animSpeedZ > 0)
            {
                animSpeedZ -= _stoppingSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 2);
            }
            if (_runTime < _minRunTime) direction = _context.playerMovement.PlayerBody.forward;
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
            NormalPlayerState.SetAsCurrentState(_context.getState(typeof(NormalPlayerState)), _context);
        }
        _runTime += Time.deltaTime;
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
        _context.anim.SetTrigger("Normal_Run");
        PlayerAttackingState.SetAsCurrentState(_context.getState(typeof(PlayerAttackingState)), _context);
    }

    public static void SetAsCurrentState(PlayerState state, PlayerContext context)
    {
        (state as PlayerFastRunState).SetUpState(context);
        state.ChangeCurrentState();
    }
}