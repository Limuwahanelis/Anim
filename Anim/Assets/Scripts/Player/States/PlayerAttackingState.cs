using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerState
{
    // TODO: make sciptable object for attacks

    float _attackLength;
    float _timer;
    bool _isWaitingForAttackEnd;
    string _endAttackName;
    private PlayerState _previousState;
    public PlayerAttackingState(PlayerContext context, PlayerState previousState) : base(context)
    {
        _previousState = previousState;
    }

    public override void Update()
    {
        if(_isWaitingForAttackEnd)
        {
            if (_timer >= _attackLength)
            {
                Debug.Log("timer");
                _context.playerCombat.ResetComboCounter();
                _context.ChangePlayerState(_previousState);
            }
            _timer += Time.deltaTime;
        }

    }

    public override void SetUpState()
    {
        _context.anim.SetTrigger("Attack");
        _attackLength = _context.animationManager.GetAnimationLength("Attack 1");
        StartWaitingForAttackEnd("Attack 1");
    }
    public override void InterruptState()
    {
     
    }
    public override void Attack()
    {
        _context.playerCombat.PerformNextAttackInCombo(this);
    }
    public void StartWaitingForAttackEnd(string attackname)
    {
        if (_isWaitingForAttackEnd) return;
        _endAttackName = attackname;
        _isWaitingForAttackEnd = true;
    }
    public void ResetTimer()
    {
        _isWaitingForAttackEnd = false;
        _timer = 0;
    }
}