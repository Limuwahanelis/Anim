using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext
{
    public Animator anim;
    public AnimationManager animationManager;
    public PlayerMovement playerMovement;
    public Action<PlayerState> ChangePlayerState;
    public PlayerChecks playerChecks;
    public CorutineHolder corutineHolder;
    public PlayerCombat playerCombat;
}
