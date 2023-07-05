using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour,IDamagable
{
    [SerializeField] IntReference maxHP;
    [SerializeField] IntReference currentHP;
    public UnityAction OnHitEvent;
    public UnityAction OnDeathEvent;
    // Start is called before the first frame update
    protected void Start()
    {
        currentHP.value = maxHP.value;
    }
    public virtual void TakeDamage(int dmg)
    {
        Debug.Log("Hit");
        currentHP.value -= dmg;
        OnHitEvent?.Invoke();
        if (currentHP.value <= 0) Kill();
    }

    public virtual void Kill()
    {
        if (OnDeathEvent == null)
        {
            Destroy(gameObject);
        }
        else OnDeathEvent.Invoke();
    }
}
