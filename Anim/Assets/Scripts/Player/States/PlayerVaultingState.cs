using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVaultingState : PlayerState
{
    Vector3 _playerPos;
    Vector3 _targetPos;
    float t = 0;
    float _distance;
    float _speed;
    public PlayerVaultingState(GetState function) : base(function)
    {

    }
    public override void Update()
    {

        _context.playerMovement.SetPosition(Vector3.Lerp(_playerPos, _targetPos, t));
        t+=Time.deltaTime*((Vector3.Distance(_targetPos, _playerPos) / (0.3f/0.7f))/_speed);
        if (t >= 1) NormalPlayerState.SetAsCurrentState( _context);
    }

    public void SetUpState(PlayerContext context, Vector3 targetPos)
    {
        base.SetUpState(context);
        _context.anim.SetFloat("SpeedZ", 0);
        t = 0;
        _targetPos = targetPos;
        _playerPos = _context.playerMovement.PlayerPosition;
        _context.playerMovement.PlayerRB.isKinematic = true;
        _context.playerMovement.PlayerRB.useGravity = false;
        _distance = Vector3.Distance(_targetPos, _playerPos);
        _speed = (_distance / (0.5f / 0.7f));
        _context.anim.SetTrigger("Vault");
    }

    public override void InterruptState()
    {
        _context.playerMovement.PlayerRB.isKinematic = false;
        _context.playerMovement.PlayerRB.useGravity = true;
    }

    public static void SetAsCurrentState(PlayerContext context, Vector3 targetPos)
    {
        PlayerState state = _getType(typeof(PlayerVaultingState));
        (state as PlayerVaultingState).SetUpState(context,targetPos);
        state.ChangeCurrentState();
    }
}