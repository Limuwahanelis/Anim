using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Player : MonoBehaviour
{
    [Header("Debug"), SerializeField] bool _printState;


    public PlayerState CurrentPlayerState => _currentPlayerState;
    public Animator anim => _anim;
    public AnimationManager animManager => _animManager;
    [SerializeField] Animator _anim;
    [SerializeField] PlayerVaulting _vaulting;
    [SerializeField] AnimationManager _animManager;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerCombat _playerCombat;
    [SerializeField] PlayerChecks _playerChecks;
    [SerializeField] CorutineHolder _corutineHolder;
    [SerializeField] StaminaBar _staminaBar;
    [SerializeField] MaterializeObject materializeObject;
    [SerializeField] PlayerClimbing _playerClimbing;
    private PlayerState _currentPlayerState;
    private PlayerContext _context;
    private Dictionary<Type, PlayerState> playerStates = new Dictionary<Type, PlayerState>();
    // Start is called before the first frame update
    void Start()
    {
        List<Type> states = AppDomain.CurrentDomain.GetAssemblies().SelectMany(domainAssembly => domainAssembly.GetTypes())
            .Where(type => typeof(PlayerState).IsAssignableFrom(type) && !type.IsAbstract).ToArray().ToList();
        

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
            staminaBar = _staminaBar,
            materializeObject = materializeObject,
            playerClimbing = _playerClimbing,
            playerVaulting = _vaulting,
        };
        PlayerState.GetState getState = GetState;
        foreach (Type state in states)
        {
            playerStates.Add(state, (PlayerState)Activator.CreateInstance(state, getState));
            //playerStates[state].SetUpState(_context);
        }
        _currentPlayerState = GetState(typeof(NormalPlayerState));
        _currentPlayerState.SetUpState(_context);
    }

    public PlayerState GetState(Type state)
    {
        return playerStates[state];
    }
    // Update is called once per frame
    void Update()
    {
        _currentPlayerState.Update();
    }
    private void FixedUpdate()
    {
        _currentPlayerState.FixedUpdate();
    }
    public void ChangeState(PlayerState newState)
    {
        if(_printState) Debug.Log(newState.GetType());
        _currentPlayerState.InterruptState();
        _currentPlayerState = newState;
    }
}
