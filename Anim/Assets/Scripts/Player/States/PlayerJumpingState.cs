using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerState
{
    bool _jumped = false;
    public PlayerJumpingState(PlayerContext context) : base(context)
    {

    }

    public override void Update()
    {
        if(_context.animationManager.GetAnimationCurrentDuration("Jump")>0.18f && !_jumped)
        {
            _jumped = true;
            _context.playerMovement.Jump();
        }
        //if (_context.animationManager.GetAnimationCurrentDuration("Jump") >= 0.25f && )
        if(!_context.playerChecks.IsTouchingGround && _jumped)
        {
            _context.anim.SetBool("IsFalling", true);
            _context.ChangePlayerState?.Invoke(new PlayerFallingState(_context));
        }
    }

    public override void SetUpState()
    {

        _context.anim.SetTrigger("Jump");
        _context.anim.SetBool("IsOnGround", false);

    }

    public override void InterruptState()
    {
        
    }
}