using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Debug"), SerializeField] bool _printState;


    public PlayerState CurrentPlayerState => _currentPlayerState;
    public Animator anim => _anim;
    public AnimationManager animManager => _animManager;
    [SerializeField] Animator _anim;
    [SerializeField] AnimationManager _animManager;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerCombat _playerCombat;
    [SerializeField] PlayerChecks _playerChecks;
    [SerializeField] CorutineHolder _corutineHolder;
    [SerializeField] StaminaBar _staminaBar;
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
            playerChecks = _playerChecks,
            corutineHolder = _corutineHolder,
            playerCombat = _playerCombat,
            staminaBar = _staminaBar
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
        if(_printState) Debug.Log(newState.GetType());
        _currentPlayerState.InterruptState();
        newState.SetUpState();
        _currentPlayerState = newState;
    }
}
