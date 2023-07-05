using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    [SerializeField] protected int _dmg;
    protected bool _isCheckingForCollisions;
    protected List<IDamagable> _damagables=new List<IDamagable>();

    public void SetCheckForCollisions(bool value)
    {
        _isCheckingForCollisions = value;
        if (!value) _damagables.Clear();
    }
}
