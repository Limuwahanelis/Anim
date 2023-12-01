using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class NormalPlayerState : PlayerState
{
    float _stoppingSpeedZ = 3f;
    float _accelerationSpeedZ = 3f;
    float animSpeedZ = 0;
    bool _isMoving;
    bool _isVaulting;
    Vector2 _movingDirection;
    public NormalPlayerState(GetState function) : base(function) 
    {

    }
    
    public override void Update()
    {
        if( _context.playerClimbing.MoveHandTowardsWall(_movingDirection)>=1)
        {

            PlayerJumpingOnWallToClimb.SetAsCurrentState( _context);
        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        _stoppingSpeedZ = 3f;
        _accelerationSpeedZ = 3f;
        _isMoving= false;
        _isVaulting= false;
        animSpeedZ = _context.anim.GetFloat("SpeedZ");
        _context.staminaBar.StartRegeneratingStamina();
    }
    public override void Move(Vector2 direction)
    {
       // if (_isMoving) return;
        _movingDirection = direction;
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
        //animSpeedZ = 2;
        if (direction != Vector2.zero)
        {
            Vector3 targetPos;
            if ( _context.playerVaulting.CheckVault(out targetPos))
            {
                PlayerVaultingState.SetAsCurrentState( _context, targetPos);
                return;
            }
        }
        if(!_context.playerChecks.IsNearGround && !_context.playerChecks.IsTouchingGround)
        {
            PlayerFallingState.SetAsCurrentState(_context);
            return;
        }
         _context.playerMovement.Move(direction, PlayerMovement.MoveState.RUN);

        _context.anim.SetFloat("SpeedZ", animSpeedZ);
    }
    public override void Jump()
    {
        PlayerJumpingState.SetAsCurrentState( _context, _isMoving);
    }

    public override void InterruptState()
    {
    }
    public override void Attack()
    {
        //ChangeState(typeof(PlayerAttackingState));
        PlayerAttackingState.SetAsCurrentState(_context);

    }
    public override void ChangeMove() => PlayerWalkingState.SetAsCurrentState( _context);
    public override void Dash() => PlayerFastRunState.SetAsCurrentState( _context);

    public static void SetAsCurrentState( PlayerContext context)
    {
        PlayerState state = _getType(typeof(NormalPlayerState));
        (state as NormalPlayerState).SetUpState(context);
        state.ChangeCurrentState();
    }
}