using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAttackingState : PlayerState
{
    // TODO: make sciptable object for attacks
    float _attackSpeed;
    float _attackLength;
    float _timer;
    bool _isWaitingForAttackEnd;
    int _combatAnimLayer;
    float _transitionTime=0.2f;
    bool _isTransitioning=false;
    float _stoppingSpeedZ = 3f;
    float _accelerationSpeedZ = 3f;
    float animSpeedZ = 0;
    Coroutine _transitionCor;
    public PlayerAttackingState() : base()
    {
        
    }
    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
        if(_transitionCor != null)
        {
            _context.corutineHolder.StopCoroutine(_transitionCor);
            _transitionCor = null;
        }
        animSpeedZ = 0;
        _combatAnimLayer = _context.anim.GetLayerIndex("Combat");
        _context.materializeObject.InstantlyMaterialize();
        _context.anim.SetLayerWeight(_combatAnimLayer, 1);
        _context.playerMovement.Move(Vector2.zero, PlayerMovement.MoveState.WALK);
        _context.animationManager.PlayAnimation("Attack 1","Combat");
        _attackLength = _context.animationManager.GetAnimationLength("Attack 1","Combat");
        _attackSpeed = _context.animationManager.GetAnimationSpeed("Attack 1","Combat");
        _isTransitioning = false;
        _timer = 0;
        StartWaitingForAttackEnd("Attack 1");
    }
    public override void Update()
    {
        if (_isWaitingForAttackEnd)
        {
            if (_timer >= _attackLength / _attackSpeed)
            {
                //_transitionCor=_context.corutineHolder.StartCoroutine(transitionCor());
                _context.anim.SetLayerWeight(_combatAnimLayer, 0);
                NormalPlayerState.SetAsCurrentState(_context.getState(typeof(NormalPlayerState)), _context);
            }
            _timer += Time.deltaTime;
        }

    }


    public override void Move(Vector2 direction)
    {
        if (direction.x == 0 && direction.y == 0)
        {
            if (animSpeedZ > 0)
            {
                animSpeedZ -= _stoppingSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 2);
            }
        }
        else
        {
            if (math.abs(direction.x) > 0)
            {
                animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 2);
            }

            if (math.abs(direction.y) > 0)
            {
                animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
                animSpeedZ = math.clamp(animSpeedZ, 0, 2);
            }
        }
        _context.anim.SetFloat("SpeedZ", animSpeedZ);
    }
    public override void InterruptState()
    {
        _context.playerCombat.MoveWeaponToSeath();
        _context.materializeObject.Materialize();
        _context.playerCombat.ResetComboCounter();
        _context.animationManager.PlayAnimation("Attack Empty","Combat");
        _context.anim.SetLayerWeight(_combatAnimLayer, 0);
    }
    public override void Attack()
    {
        _context.playerCombat.PerformNextAttackInCombo(this);
    }
    public void StartWaitingForAttackEnd(string attackname)
    {
        if (_isWaitingForAttackEnd) return;
        _attackLength = _context.animationManager.GetAnimationLength(attackname,"Combat");
        _attackSpeed = _context.animationManager.GetAnimationSpeed(attackname, "Combat");
        _isWaitingForAttackEnd = true;
    }
    public void ResetTimer()
    {
        _isWaitingForAttackEnd = false;
        _timer = 0;
    }
    public override void Dash()
    {
        PlayerFastRunState.SetAsCurrentState(_context.getState(typeof(PlayerFastRunState)), _context);
    }
    IEnumerator transitionCor()
    {
        if (_isTransitioning) yield break;
        float t = 0f;
        _isTransitioning = true;
        while (t<1)
        {
            t += (Time.deltaTime/_transitionTime);
            _context.anim.SetLayerWeight(_combatAnimLayer,1 - t);
            yield return null;
        }
        _context.anim.SetLayerWeight(_combatAnimLayer, 0);
        NormalPlayerState.SetAsCurrentState(_context.getState(typeof(NormalPlayerState)), _context);
    }
    public static void SetAsCurrentState(PlayerState state, PlayerContext context)
    {
        (state as PlayerAttackingState).SetUpState(context);
        state.ChangeCurrentState();
    }
}