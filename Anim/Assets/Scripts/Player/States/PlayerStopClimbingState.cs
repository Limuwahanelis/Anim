using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopClimbingState : PlayerState
{
    public PlayerStopClimbingState(GetState function) : base(function)
    {

    }
    public override void Update()
    {
        if (!_context.playerChecks.IsTouchingGround)
        {
            _context.anim.SetBool("IsFalling", true);
            PlayerFallingState.SetAsCurrentState( _context);
        }
        else
        {
            _context.anim.SetBool("IsOnGround", true);
            NormalPlayerState.SetAsCurrentState( _context);

        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
    }

    public override void InterruptState()
    {

    }
    public static void SetAsCurrentState( PlayerContext context)
    {
        PlayerState state = _getType(typeof(PlayerStopClimbingState));
        (state as PlayerStopClimbingState).SetUpState(context);
        state.ChangeCurrentState();
    }
}