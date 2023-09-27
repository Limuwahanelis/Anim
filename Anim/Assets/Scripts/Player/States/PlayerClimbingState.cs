using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : PlayerState
{
    bool _isMoving;
    float _distance = 2f;
    bool laa;
    public PlayerClimbingState(PlayerContext context) : base(context)
    {

    }

    public override void Update()
    {
        if(laa) _context.playerClimbing.RotateTowardsWall();
    }

    public override void SetUpState()
    {
        _context.playerClimbing.OnStartClimbing += () => { laa = true; };
        _context.playerClimbing.MoveToStartClimbingPos();
        _context.anim.SetTrigger("Climb");
    }

    public override void InterruptState()
    {
     
    }
    public override void Move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _context.corutineHolder.StartCoroutine(ClimbCor(direction));
            _context.playerClimbing.Cycle(_context.playerMovement.ClimbSpeed/2);
        }

    }

    private IEnumerator ClimbCor(Vector2 direction)
    {
        if (_isMoving) yield break;
        Debug.Log("start");
        _isMoving = true;
        float _traveledDistance = 0f;
        while(_traveledDistance < _distance)
        {
            _context.playerMovement.Climb(direction);
            _traveledDistance+=_context.playerMovement.ClimbSpeed*Time.deltaTime;
            yield return null;
        }
        Debug.Log("end");
        _isMoving =false;
    }
}