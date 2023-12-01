using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerState
{
    float _landingTime = 0f;
    bool _touchedGround = false;
    public PlayerFallingState(GetState function) : base(function)
    {

    }


    public override void Update()
    {
        if(_touchedGround)
        {
            _landingTime+=Time.deltaTime;
        }
        if(_context.playerChecks.IsTouchingGround && !_touchedGround)
        {
            _context.anim.SetBool("IsFalling", false);
            _context.anim.SetBool("IsOnGround", true);
             _touchedGround = true;
        }
        if(_landingTime>=0.1f)
        {
            NormalPlayerState.SetAsCurrentState( _context);
        }
        
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
         _landingTime = 0f;
         _touchedGround = false;
        _context.playerMovement.PlayerRB.useGravity = true;
        _context.anim.SetBool("IsFalling", true);
        _context.anim.SetBool("IsOnGround", false);
    }

    public override void InterruptState()
    {
        _context.playerMovement.PlayerRB.useGravity = false;
    }
    public override void Move(Vector2 direction)
    {
        if (_context.playerClimbing.CheckForClimbFromAir(direction))
        {
            _context.anim.SetBool("IsFalling", false);
            _context.anim.SetBool("Start_Climb", true);
            _context.playerClimbing.StartClimbingFromAir();
            PlayerClimbingState.SetAsCurrentState( _context);
        }
    }
    public static void SetAsCurrentState(PlayerContext context)
    {
        PlayerState state = _getType(typeof(PlayerFallingState));
        (state as PlayerFallingState).SetUpState(context);
        state.ChangeCurrentState();
    }
}