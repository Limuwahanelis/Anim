using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerState
{
    bool _jumped = false;
    bool _isMoving;
    public PlayerJumpingState(PlayerContext context,bool isMoving) : base(context)
    {
        _isMoving = isMoving;
    }

    public override void Update()
    {
        if (!_context.playerChecks.IsTouchingGround && _jumped)
        {
            _context.anim.SetBool("IsFalling", true);
            _context.ChangePlayerState?.Invoke(new PlayerFallingState(_context));
        }
    }

    public override void SetUpState()
    {

        _context.anim.SetTrigger("Jump");
        _context.anim.SetBool("IsOnGround", false);
        _context.playerMovement.Jump(_isMoving);
        _jumped = true;
    }

    public override void InterruptState()
    {
        
    }
}