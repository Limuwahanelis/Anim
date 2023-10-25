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
    string _endAttackName;
    public PlayerAttackingState(PlayerContext context) : base(context)
    {
    }
    public override void SetUpState()
    {
        _context.materializeObject.InstantlyMaterialize();
        _context.playerMovement.Move(Vector2.zero, PlayerMovement.MoveState.WALK);
        _context.anim.SetTrigger("Attack");
        _attackLength = _context.animationManager.GetAnimationLength("Attack 1");
        _attackSpeed = _context.animationManager.GetAnimationSpeed("Attack 1");
        StartWaitingForAttackEnd("Attack 1");
    }
    public override void Update()
    {
        if(_isWaitingForAttackEnd)
        {
            if (_timer >= _attackLength/_attackSpeed)
            {
                _context.ChangePlayerState(new NormalPlayerState(_context));
            }
            _timer += Time.deltaTime;
        }

    }


    public override void Move(Vector2 direction)
    {
        //SetAnimation(direction);
    }
    public override void InterruptState()
    {
        _context.playerCombat.MoveWeaponToSeath();
        _context.materializeObject.Materialize();
        _context.playerCombat.ResetComboCounter();
    }
    //private void SetAnimation(Vector2 direction)
    //    {
    //        if (direction.x == 0)
    //        {
    //            if (animSpeedX > 0)
    //            {
    //                animSpeedX -= _stoppingSpeedX * Time.deltaTime;
    //                animSpeedX = math.clamp(animSpeedX, 0, 1);
    //            }
    //            else
    //            {
    //                animSpeedX += _stoppingSpeedX * Time.deltaTime;
    //                animSpeedX = math.clamp(animSpeedX, -1, 0);
    //            }
    //        }
    //        else
    //        {
    //            if (direction.x > 0)
    //            {
    //                animSpeedX += _accelerationSpeedX * Time.deltaTime;
    //                animSpeedX = math.clamp(animSpeedX, -1, 1);
    //            }
    //            else
    //            {
    //                animSpeedX -= _accelerationSpeedX * Time.deltaTime;
    //                animSpeedX = math.clamp(animSpeedX, -1, 1);
    //            }
    //        }

    //        if (direction.y == 0)
    //        {
    //            if (animSpeedZ > 0)
    //            {
    //                animSpeedZ -= _stoppingSpeedZ * Time.deltaTime;
    //                animSpeedZ = math.clamp(animSpeedZ, 0, 1);
    //            }
    //            else
    //            {
    //                animSpeedZ += _stoppingSpeedZ * Time.deltaTime;
    //                animSpeedZ = math.clamp(animSpeedZ, -1, 0);
    //            }
    //        }
    //        else
    //        {
    //            if (direction.y > 0)
    //            {
    //                animSpeedZ += _accelerationSpeedZ * Time.deltaTime;
    //                animSpeedZ = math.clamp(animSpeedZ, -1, 1);
    //            }
    //            else
    //            {
    //                animSpeedZ -= _accelerationSpeedZ * Time.deltaTime;
    //                animSpeedZ = math.clamp(animSpeedZ, -1, 1);
    //            }
    //        }


    //        _context.anim.SetFloat("SpeedX", animSpeedX);
    //        _context.anim.SetFloat("SpeedZ", animSpeedZ);
    //    }
    public override void Attack()
    {
        _context.playerCombat.PerformNextAttackInCombo(this);
    }
    public void StartWaitingForAttackEnd(string attackname)
    {
        if (_isWaitingForAttackEnd) return;
        _endAttackName = attackname;
        _attackLength = _context.animationManager.GetAnimationLength(attackname);
        _attackSpeed = _context.animationManager.GetAnimationSpeed(attackname);
        _isWaitingForAttackEnd = true;
    }
    public void ResetTimer()
    {
        _isWaitingForAttackEnd = false;
        _timer = 0;
    }
    public override void Dash()
    {
        _context.ChangePlayerState(new PlayerFastRunState(_context));
    }
}