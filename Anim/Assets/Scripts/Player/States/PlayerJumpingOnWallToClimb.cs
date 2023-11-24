using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingOnWallToClimb : PlayerState
{
    float _timeToWait = 0.2f;
    float _time = 0f;
    bool _fired;
    public PlayerJumpingOnWallToClimb() : base()
    {

    }
    public override void Update()
    {
        if(_time < _timeToWait)
        {
            _time+=Time.deltaTime;
        }
        else if(!_fired)
        {
            _fired = true;
            _context.playerClimbing.MoveToStartClimbingPos();
        }
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
         _time = 0f;
         _fired=false;
        _context.playerClimbing.OnStartClimbing += MoveToClimbingState;
        _context.anim.SetTrigger("Start_Climb");
        _context.anim.SetBool("IsOnGround", false);
    }
    private void MoveToClimbingState()
    {
        PlayerClimbingState.SetAsCurrentState(_context.getState(typeof(PlayerClimbingState)), _context);
    }
    public override void InterruptState()
    {
        _context.playerClimbing.OnStartClimbing -= MoveToClimbingState;
    }
    public static void SetAsCurrentState(PlayerState state, PlayerContext context)
    {
        (state as PlayerJumpingOnWallToClimb).SetUpState(context);
        state.ChangeCurrentState();
    }
}