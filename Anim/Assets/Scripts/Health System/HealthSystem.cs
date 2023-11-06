using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ElementalStatus))]
public class HealthSystem : MonoBehaviour,IDamagable
{
    [SerializeField] ElementalStatus _elementalStatus;
    [SerializeField] FloatingTextPool _damageNumbersPool;
    [SerializeField] IntReference maxHP;
    [SerializeField] IntReference currentHP;
    [SerializeField] HealthBar _healthBar;
    public UnityAction<ReactionCalculator.Element> OnHitEvent;
    public UnityAction OnDeathEvent;
    // Start is called before the first frame update
    protected void Start()
    {
        _elementalStatus=GetComponent<ElementalStatus>();
        currentHP.value = maxHP.value;
        if (_healthBar != null) _healthBar.SetMaxHealth(maxHP.value);
    }
    public virtual void TakeDamage(DamageInfo damageInfo)
    {
        int recievedDmg;
        Color damageColor;
        Color reactionColor;
        ReactionCalculator.ElementalReaction elementalReaction = _elementalStatus.SetElement(damageInfo,out recievedDmg,out damageColor,out reactionColor);
        FloatingText num= _damageNumbersPool.GetText();
        if (elementalReaction != ReactionCalculator.ElementalReaction.NONE)
        {
            FloatingText reaction = _damageNumbersPool.GetText();
            reaction.SetText(elementalReaction.ToString(), reactionColor, transform.parent);
        }
        num.SetText(recievedDmg, damageColor, transform.parent);
        currentHP.value -= recievedDmg;
        if (_healthBar != null) _healthBar.ReduceHP(recievedDmg);
        OnHitEvent?.Invoke(damageInfo.element);
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
