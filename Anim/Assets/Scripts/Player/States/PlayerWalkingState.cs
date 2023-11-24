using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerWalkingState : PlayerState
{
    float _stoppingSpeedZ = 0.5f;
    float _accelerationSpeedZ = 2.5f;
    float animSpeedZ = 0;
    bool _isMoving;
    public PlayerWalkingState() : base()
    {

    }
    public override void Update()
    {

    }

    public override void SetUpState(PlayerContext context)
    {
        _context = context;
         animSpeedZ = 0;
         _isMoving=false;
        animSpeedZ = _context.anim.GetFloat("SpeedZ");
        _context.staminaBar.StartRegeneratingStamina();
    }
    public override void Move(Vector2 direction)
    {
        if (direction.x == 0 && direction.y == 0)
        {
            _isMoving = false;
            if (animSpeedZ > 1)
            {
                animSpeedZ -= _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 1, 2);
            }
            else if (animSpeedZ > 0)
            {
                animSpeedZ -= _stoppingSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 1);
            }
        }
        else
        {
            _isMoving = true;
            if (animSpeedZ > 1)
            {
                animSpeedZ -= _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 1, 2);
            }
            else
            {
                if (math.abs(direction.x) > 0)
                {
                    animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
                    animSpeedZ = math.clamp(animSpeedZ, 0, 1);
                }

                if (math.abs(direction.y) > 0)
                {
                    animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
                    animSpeedZ = math.clamp(animSpeedZ, 0, 1);
                }
            }
        }
        if (direction != Vector2.zero)
        {
            Vector3 targetPos;
            if (_context.playerVaulting.CheckVault(out targetPos))
            {
                PlayerVaultingState.SetAsCurrentState(_context.getState(typeof(PlayerVaultingState)), _context, targetPos);
                return;
            }
        }
        if (!_context.playerChecks.IsNearGround && !_context.playerChecks.IsTouchingGround)
        {
            PlayerFallingState.SetAsCurrentState(_context.getState(typeof(PlayerFallingState)), _context);
            return;
        }

        _context.playerMovement.Move(direction, PlayerMovement.MoveState.WALK);

        _context.anim.SetFloat("SpeedZ", animSpeedZ);
    }
    public override void Jump()
    {
        _context.ChangePlayerState?.Invoke(new PlayerJumpingState(_context, _isMoving));
    }

    public override void InterruptState()
    {

    }
    public override void Attack()
    {
        PlayerAttackingState.SetAsCurrentState(_context.getState(typeof(PlayerAttackingState)), _context);
    }

    public override void ChangeMove()
    {
        _context.anim.SetTrigger("Walk_Run");
        NormalPlayerState.SetAsCurrentState(_context.getState(typeof(NormalPlayerState)), _context);
    }

    public static void SetAsCurrentState(PlayerState state, PlayerContext context)
    {
        (state as PlayerWalkingState).SetUpState(context);
        state.ChangeCurrentState();
    }
}