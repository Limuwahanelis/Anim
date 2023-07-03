using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlayerState : PlayerState
{
    public NormalPlayerState(PlayerContext context) : base(context)
    {

    }

    public override void Update()
    {

    }

    public override void SetUpState()
    {

    }
    public override void Move(Vector2 direction)
    {
        float animSpeedZ = direction.y;
        float animSpeedX = direction.x;

        if (animSpeedX > 0.5f) animSpeedX = 1f;
        if (animSpeedX < -0.5f) animSpeedX = -1f;

        if (animSpeedZ > 0.5f) animSpeedZ = 1f;
        if (animSpeedZ < -0.5f) animSpeedZ = -1f;

        _context.playerMovement.Move(direction);

        //animSpeedZ = animSpeedZ > 0 ? 1 : -1;
        //animSpeedX = animSpeedX > 0 ? 1 : -1;

        _context.anim.SetFloat("SpeedX",animSpeedX);
        _context.anim.SetFloat("SpeedZ", animSpeedZ);
    }
    public override void Jump()
    {
        _context.ChangePlayerState?.Invoke(new PlayerJumpingState(_context));
    }

    public override void InterruptState()
    {

    }
}