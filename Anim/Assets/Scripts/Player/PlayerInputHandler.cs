using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] GameObject tester;
    [SerializeField] PlayerTeleport teleportSkill;
    private bool _isFocused = false;
    Vector2 move;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _player.CurrentPlayerState.Move(move);
    }

    void OnMove(InputValue inputValue)
    {
        move = inputValue.Get<Vector2>();
    }
    void OnJump()
    {
        Debug.Log("Jump");
        _player.CurrentPlayerState.Jump();
    }
    void OnFocus(InputValue inputValue)
    {
        Debug.Log(inputValue.Get<float>());
        _isFocused = inputValue.Get<float>()>0?true:false;
        teleportSkill.enabled = _isFocused;
    }
    void OnDodge(InputValue inputValue)
    {
        _player.CurrentPlayerState.Dodge();
    }
    void OnFire(InputValue inputValue)
    {
        if(_isFocused) teleportSkill.Perform();
        else
        {
            _player.CurrentPlayerState.Attack();
        }

    }

    void OnWalk_Swap(InputValue inputValue)
    {
        Debug.Log(inputValue.Get<float>());
        _player.CurrentPlayerState.ChangeMove();
    }
}
