using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NormalPlayerState : PlayerState
{
    float _stoppingSpeedZ = 3f;
    float _accelerationSpeedZ = 3f;
    float animSpeedZ = 0;
    bool _isMoving;
    bool _isVaulting;
    Vector2 _movingDirection;
    public NormalPlayerState(PlayerContext context) : base(context)
    {
        animSpeedZ = _context.anim.GetFloat("SpeedZ");
       // _context.playerClimbing.OnStartClimbing += ChangeToClimbingState;
    }

    public override void Update()
    {
        if( _context.playerClimbing.MoveHandTowardsWall(_movingDirection)>=1)
        {
            _context.ChangePlayerState(new PlayerJumpingOnWallToClimb(_context));
        }
    }

    public override void SetUpState()
    {
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

        if (direction != Vector2.zero)
        {
            Vector3 targetPos;
            if ( _context.playerVaulting.CheckVault( out targetPos))
            {
                
                _context.ChangePlayerState?.Invoke(new PlayerVaultingState(_context,targetPos));
                return;
            }
        }
        if(!_context.playerChecks.IsNearGround && !_context.playerChecks.IsTouchingGround)
        {
            _context.ChangePlayerState?.Invoke(new PlayerFallingState(_context));
            return;
        }
         _context.playerMovement.Move(direction, PlayerMovement.MoveState.RUN);

        _context.anim.SetFloat("SpeedZ", animSpeedZ);
    }
    public override void Jump()
    {
        _context.ChangePlayerState?.Invoke(new PlayerJumpingState(_context,_isMoving));
    }

    public override void InterruptState()
    {
    }
    public override void Attack()
    {
        _context.ChangePlayerState(new PlayerAttackingState(_context));
    }
    public override void ChangeMove()=>_context.ChangePlayerState(new PlayerWalkingState(_context));
    public override void Dash()=> _context.ChangePlayerState(new PlayerFastRunState(_context));
}