using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext
{
    public Animator anim;
    public AnimationManager animationManager;
    public PlayerMovement playerMovement;
    public PlayerVaulting playerVaulting;
    public Action<PlayerState> ChangePlayerState;
    public delegate PlayerState GetState(Type state);
    public GetState getState;
    public PlayerChecks playerChecks;
    public CorutineHolder corutineHolder;
    public PlayerCombat playerCombat;
    public StaminaBar staminaBar;
    public MaterializeObject materializeObject;
    public PlayerClimbing playerClimbing;
}
