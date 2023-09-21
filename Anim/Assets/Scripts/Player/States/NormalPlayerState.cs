using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NormalPlayerState : PlayerState
{
    float _stoppingSpeedZ = 3f;
    float _accelerationSpeedZ = 3f;
    float animSpeedZ = 0;
    public NormalPlayerState(PlayerContext context) : base(context)
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
            if (animSpeedZ > 0)
            {
                animSpeedZ -= _stoppingSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 2);
            }
        }
        else
        {
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
        
         _context.playerMovement.Move(direction, PlayerMovement.MoveState.RUN);

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
        _context.ChangePlayerState(new PlayerWalkingState(_context));

    }
}