using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerContext _context;
    public PlayerState(PlayerContext context)
    {
        _context = context;
    }
    public abstract void InterruptState();
    public abstract void Update();
    public abstract void SetUpState();
    public virtual void Move(Vector2 direction) { }
    public virtual void Jump() { }
    public virtual void Attack() { }

    public virtual void Dodge() { }
}
