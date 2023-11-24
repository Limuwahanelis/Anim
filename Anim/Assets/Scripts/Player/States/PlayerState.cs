using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerContext _context;
    public PlayerState() { }
    public PlayerState(PlayerContext context)
    {
        _context = context;
    }
    public abstract void InterruptState();
    public abstract void Update();
    public virtual void FixedUpdate() { }
    public virtual void SetUpState(PlayerContext context)
    {
        _context = context;
    }
    public virtual void Move(Vector2 direction) { }
    public virtual void Jump() { }
    public virtual void Attack() { }

    public virtual void Dash() { }

    public virtual void ChangeMove() { }

    public virtual void Drop() { }

    public virtual void ChangeCurrentState()
    {
        _context.ChangePlayerState(this);
    }
}

