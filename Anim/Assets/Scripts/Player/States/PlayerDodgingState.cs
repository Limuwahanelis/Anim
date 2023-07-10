using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerState
{
    PlayerState _previousState;
    Vector2 _direction;
    float _dodgeAnimationLength;
    float _timer = 0;
    public PlayerDodgingState(PlayerContext context, PlayerState previousState, Vector2 dogeDirection) : base(context)
    {
        _direction = dogeDirection;
        _previousState = previousState;
        _dodgeAnimationLength = _context.animationManager.GetAnimationLength("Dodge");
    }

    public override void Update()
    {
        if (_timer >= _dodgeAnimationLength)
        {
            _context.playerMovement.RotatePlayerBack();
            _context.ChangePlayerState(_previousState);
        }
            
        _timer += Time.deltaTime;
    }

    public override void SetUpState()
    {
        _context.playerMovement.Roll(_direction);
        _context.anim.SetTrigger("Roll");
    }

    public override void InterruptState()
    {
     
    }
}