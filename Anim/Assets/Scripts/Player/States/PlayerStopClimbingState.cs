using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopClimbingState : PlayerState
{
    public PlayerStopClimbingState() : base()
    {

    }
    public override void Update()
    {
        if (!_context.playerChecks.IsTouchingGround)
        {
            _context.anim.SetBool("IsFalling", true);
            PlayerFallingState.SetAsCurrentState(_context.getState(typeof(NormalPlayerState)), _context);
        }
        else
        {
            _context.anim.SetBool("IsOnGround", true);
            NormalPlayerState.SetAsCurrentState(_context.getState(typeof(NormalPlayerState)), _context);

        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
    }

    public override void InterruptState()
    {

    }
    public static void SetAsCurrentState(PlayerState state, PlayerContext context)
    {
        (state as PlayerStopClimbingState).SetUpState(context);
        state.ChangeCurrentState();
    }
}