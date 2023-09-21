using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerWalkingState : PlayerState
{
    float _stoppingSpeedZ = 0.5f;
    float _accelerationSpeedZ = 2.5f;
    float animSpeedZ = 0;
    public PlayerWalkingState(PlayerContext context) : base(context)
    {
        animSpeedZ = _context.anim.GetFloat("SpeedZ");
    }

    public override void Update()
    {

    }

    public override void SetUpState()
    {

    }
    public override void Move(Vector2 direction)
    {
        if (direction.x == 0 && direction.y == 0)
        {
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

        _context.playerMovement.Move(direction, PlayerMovement.MoveState.WALK);

        _context.anim.SetFloat("SpeedZ", animSpeedZ);
    }
    public override void Jump()
    {
        _context.ChangePlayerState?.Invoke(new PlayerJumpingState(_context));
    }

    public override void InterruptState()
    {

    }
    public override void Attack()
    {
        _context.ChangePlayerState(new PlayerCombatState(_context));
    }

    public override void ChangeMove()
    {
        _context.anim.SetTrigger("Walk_Run");
        _context.ChangePlayerState(new NormalPlayerState(_context));
    }
}