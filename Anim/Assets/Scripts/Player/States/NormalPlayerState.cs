using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NormalPlayerState : PlayerState
{
    float _stoppingSpeedX = 2f;
    float _stoppingSpeedZ = 2f;
    float _accelerationSpeedX = 2.5f;
    float _accelerationSpeedZ = 2.5f;
    float animSpeedZ = 0;
    float animSpeedX = 0;
    public NormalPlayerState(PlayerContext context) : base(context)
    {

    }

    public override void Update()
    {

    }

    public override void SetUpState()
    {

    }
    public override void Move(Vector2 direction)
    {
        Debug.Log((math.atan2(direction.y , -direction.x) * 180 / Mathf.PI)-90);
        if (direction.x == 0)
        {
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
        else
        {
            if (direction.x > 0)
            {
                animSpeedX += _accelerationSpeedX * Time.deltaTime;
                animSpeedX = math.clamp(animSpeedX, -1, 1);
            }
            else
            {
                animSpeedX -= _accelerationSpeedX * Time.deltaTime;
                animSpeedX = math.clamp(animSpeedX, -1, 1);
            }
        }

        if (direction.y == 0)
        {
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
        else
        {
            if (direction.y > 0)
            {
                animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, -1, 1);
            }
            else
            {
                animSpeedZ -= _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, -1, 1);
            }
        }
        if(math.abs(animSpeedX)>0.1|| math.abs(animSpeedZ)>0.1) _context.playerMovement.Move(direction,false);


        _context.anim.SetFloat("SpeedX", animSpeedX);
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
}