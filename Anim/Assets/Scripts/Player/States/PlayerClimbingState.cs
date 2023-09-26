using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : PlayerState
{
    public PlayerClimbingState(PlayerContext context) : base(context)
    {

    }

    public override void Update()
    {

    }

    public override void SetUpState()
    {
        _context.playerClimbing.MoveToStartClimbingPos();
        _context.anim.SetTrigger("Climb");
    }

    public override void InterruptState()
    {
     
    }
}