using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingOnWallToClimb : PlayerState
{
    public PlayerJumpingOnWallToClimb(PlayerContext context) : base(context)
    {
        _context.playerClimbing.OnStartClimbing += MoveToClimbingState;
    }

    public override void Update()
    {

    }

    public override void SetUpState()
    {
        _context.playerClimbing.MoveToStartClimbingPos();
        _context.anim.SetTrigger("Start_Climb");
        _context.anim.SetBool("IsOnGround", false);
    }
    private void MoveToClimbingState()
    {
        _context.ChangePlayerState(new PlayerClimbingState(_context));
    }
    public override void InterruptState()
    {
        _context.playerClimbing.OnStartClimbing -= MoveToClimbingState;
    }
}