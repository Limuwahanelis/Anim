using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerState
{
    bool _jumped = false;
    bool _isMoving;
    public PlayerJumpingState(PlayerContext context,bool isMoving) : base(context)
    {
        
    }
    public PlayerJumpingState() : base()
    {

    }
    public override void Update()
    {
        if (!_context.playerChecks.IsTouchingGround && _jumped)
        {
            PlayerFallingState.SetAsCurrentState(_context.getState(typeof(PlayerFallingState)), _context);
        }
    }
    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);

    }
    public void SetUpState(PlayerContext context, bool isMoving)
    {
        base.SetUpState(context);
        _isMoving = isMoving;
        _context.anim.SetTrigger("Jump");
        _context.anim.SetBool("IsOnGround", false);
        _context.playerMovement.Jump(_isMoving);
        _jumped = true;
    }

    public override void InterruptState()
    {
        
    }
    public static void SetAsCurrentState(PlayerState state, PlayerContext context, bool isMoving)
    {
        (state as PlayerJumpingState).SetUpState(context,isMoving);
        state.ChangeCurrentState();
    }
}