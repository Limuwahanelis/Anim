using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerState CurrentPlayerState => _currentPlayerState;
    [SerializeField] Animator _anim;
    [SerializeField] AnimationManager _animManager;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerChecks _playerChecks;
    private PlayerState _currentPlayerState;
    private PlayerContext _context;
    // Start is called before the first frame update
    void Start()
    {
        // aa = ChangeState;
        _context = new PlayerContext
        {
            anim = _anim,
            playerMovement = _playerMovement,
            ChangePlayerState = ChangeState,
            animationManager = _animManager,
            playerChecks = _playerChecks
        };
        _currentPlayerState = new NormalPlayerState(_context);
    }

    // Update is called once per frame
    void Update()
    {
        _currentPlayerState.Update();
    }

    public void ChangeState(PlayerState newState)
    {
        Debug.Log(newState.GetType());
        _currentPlayerState.InterruptState();
        newState.SetUpState();
        _currentPlayerState = newState;
    }
}
