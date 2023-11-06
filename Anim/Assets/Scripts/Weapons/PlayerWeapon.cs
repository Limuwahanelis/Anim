using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    public ReactionCalculator.Element element;
    [SerializeField] protected int _dmg;
    protected bool _isCheckingForCollisions;
    protected List<IDamagable> _damagables=new List<IDamagable>();
    protected DamageInfo _damageInfo;
    private void Awake()
    {

    }

    public void SetCheckForCollisions(bool value)
    {
        _isCheckingForCollisions = value;
        if (value) _damagables.Clear();
    }
    public void ResetTargets()
    {
        Debug.Log("Clear");
        _damagables.Clear();
    }
}
