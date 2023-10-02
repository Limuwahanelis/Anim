using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopClimbingState : PlayerState
{
    public PlayerStopClimbingState(PlayerContext context) : base(context)
    {

    }

    public override void Update()
    {
        if (!_context.playerChecks.IsTouchingGround)
        {
            _context.anim.SetBool("IsFalling", true);
            _context.ChangePlayerState(new PlayerFallingState(_context));
        }
        else
        {
            _context.anim.SetBool("IsOnGround", true);
            _context.ChangePlayerState(new NormalPlayerState(_context));
        }
    }

    public override void SetUpState()
    {

    }

    public override void InterruptState()
    {
     
    }
}