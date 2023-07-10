using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCombatState : PlayerState
{
    Vector2 _direction;
    float _stoppingSpeedX = 1f;
    float _stoppingSpeedZ = 1f;
    float _accelerationSpeedX = 2f;
    float _accelerationSpeedZ = 2f;
    float animSpeedZ=0;
    float animSpeedX=0;
    public PlayerCombatState(PlayerContext context) : base(context)
    {

    }

    public override void Update()
    {

    }
    public override void Move(Vector2 direction)
    {
        _direction=direction;
        if (direction.x==0)
        {
            if (animSpeedX > 0)
            {
                animSpeedX -= _stoppingSpeedX * Time.deltaTime;
                animSpeedX=math.clamp(animSpeedX, 0, 1);
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

        if(direction.y==0)
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
        _context.playerMovement.Move(direction);

        _context.anim.SetFloat("SpeedX", animSpeedX);
        _context.anim.SetFloat("SpeedZ", animSpeedZ);
    }

    public override void SetUpState()
    {
        _context.anim.SetTrigger("Unseath");

    }
    public override void Attack()
    {
        _context.anim.SetTrigger("Attack");
    }
    public override void Dodge()
    {
        _context.ChangePlayerState(new PlayerDodgingState(_context, this,_direction));
    }
    public override void InterruptState()
    {
        
    }

}