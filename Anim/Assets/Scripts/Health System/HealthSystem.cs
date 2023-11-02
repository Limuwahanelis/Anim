using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour,IDamagable
{
    [SerializeField] ElementalFontSettings fontSettings;
    [SerializeField] FloatingTextPool _damageNumbersPool;
    [SerializeField] IntReference maxHP;
    [SerializeField] IntReference currentHP;
    public ReactionCalculator.Element afflictedElement;
    public UnityAction OnHitEvent;
    public UnityAction OnDeathEvent;
    // Start is called before the first frame update
    protected void Start()
    {
        currentHP.value = maxHP.value;
    }
    public virtual void TakeDamage(DamageInfo damageInfo)
    {
        //Debug.Log("Hit");
        int recievedDmg;
        ReactionCalculator.ElementalReaction elementalReaction = ReactionCalculator.CalculateDamage(afflictedElement, damageInfo, out recievedDmg);
        Debug.Log(elementalReaction);
        FloatingText num= _damageNumbersPool.GetText();
        if (elementalReaction != ReactionCalculator.ElementalReaction.NONE)
        {
            FloatingText reaction = _damageNumbersPool.GetText();
            reaction.SetText(elementalReaction.ToString(), fontSettings.elementalReactionFonts[((int)elementalReaction)],transform.parent);
        }
        num.SetText(recievedDmg,fontSettings.elementFonts[((int)damageInfo.element)],transform.parent);
        currentHP.value -= recievedDmg;
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
