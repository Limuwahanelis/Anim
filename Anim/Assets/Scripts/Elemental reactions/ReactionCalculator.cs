using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReactionCalculator
{
    public enum Element
    {
        PYRO,HYDRO,ELECTRO,CRYO,GEO,PHYSICAL
    }
    public enum ElementalReaction
    {
        NONE,VAPORIZE, OVERLOAD,MELT,ELECTROCHARGED,SUPERCONDUCT,CRYSTALIZE,FROZEN
    }
    public static ElementalReaction CalculateDamage(Element element, DamageInfo damageInfo, out int recievedDamage)
    {
        recievedDamage = damageInfo.damage;
        switch (element)
        { 
            case Element.PYRO:
                {
                    switch (damageInfo.element)
                    {
                        case Element.HYDRO:return Vaporize(damageInfo.element,damageInfo,out recievedDamage);
                        case Element.ELECTRO:return Overload(damageInfo, out recievedDamage);
                        case Element.CRYO:return Melt(damageInfo.element, damageInfo, out recievedDamage);break;
                        case Element.GEO:return Crystalize();break;
                        default: break;
                    }
                    break;
                }
            case Element.HYDRO: 
                {
                    switch(damageInfo.element)
                    {
                        case Element.PYRO:return Vaporize(damageInfo.element, damageInfo,out recievedDamage);break;
                        case Element.ELECTRO:return ElectroCharged(damageInfo, out recievedDamage); break;
                        case Element.CRYO:return Frozen(); break;
                        case Element.GEO:return Crystalize(); break;
                        default:break;
                    }
                    break;
                }
            case Element.ELECTRO:
                {
                    switch(damageInfo.element)
                    {
                        case Element.PYRO:return Overload(damageInfo, out recievedDamage); break;
                        case Element.HYDRO:return ElectroCharged(damageInfo, out recievedDamage); break;
                        case Element.CRYO:return SuperConduct(damageInfo, out recievedDamage); break;
                        case Element.GEO:return Crystalize(); break;
                    }
                    break;
                }
            case Element.CRYO:
                {
                    switch(damageInfo.element)
                    {
                        case Element.PYRO:return Melt(damageInfo.element,damageInfo, out recievedDamage); break;
                        case Element.HYDRO:return Frozen(); break;
                        case Element.ELECTRO:return SuperConduct(damageInfo, out recievedDamage); break;
                        case Element.GEO:return Crystalize(); break;
                    }
                    break;
                }
            case Element.GEO:
                {
                    switch (damageInfo.element)
                    {
                        case Element.PYRO: return Crystalize(); break;
                        case Element.HYDRO: return Crystalize(); break;
                        case Element.ELECTRO: return Crystalize(); break;
                        case Element.CRYO: return Crystalize(); break;
                    }
                    break;
                }
        }

        return 0;
    }

    private static ElementalReaction Vaporize(Element triggeringElement,DamageInfo damageInfo,out int recievedDamage)
    {
        recievedDamage = AmplifyingReaction(ElementalReaction.VAPORIZE, triggeringElement, damageInfo);
        return ElementalReaction.VAPORIZE;
    }
   
    private static ElementalReaction Overload( DamageInfo damageInfo, out int recievedDamage) {
        recievedDamage = TransformativeReaction(ElementalReaction.OVERLOAD, damageInfo);
        return ElementalReaction.OVERLOAD;
    }
    private static ElementalReaction Melt(Element triggeringElement, DamageInfo damageInfo, out int recievedDamage)
    {
        recievedDamage = AmplifyingReaction(ElementalReaction.MELT, triggeringElement, damageInfo);
        return ElementalReaction.MELT; 
    }
    private static ElementalReaction ElectroCharged(DamageInfo damageInfo, out int recievedDamage) 
    {
        recievedDamage = TransformativeReaction(ElementalReaction.ELECTROCHARGED, damageInfo);
        return ElementalReaction.ELECTROCHARGED; 
    }
    private static ElementalReaction SuperConduct(DamageInfo damageInfo, out int recievedDamage) {
        recievedDamage= TransformativeReaction(ElementalReaction.SUPERCONDUCT, damageInfo);
        return ElementalReaction.SUPERCONDUCT; 
    }
    private static ElementalReaction Crystalize() { return ElementalReaction.CRYSTALIZE; }

    private static ElementalReaction Frozen() { return ElementalReaction.FROZEN; }

    //player level is set in sword as 10. should be made into multipler
    private static int TransformativeReaction(ElementalReaction reaction, DamageInfo damageInfo)
    {
        float reactionMultiplier = 0;
        switch(reaction)
        {
            case ElementalReaction.SUPERCONDUCT: reactionMultiplier = 0.5f;break;
            case ElementalReaction.ELECTROCHARGED: reactionMultiplier = 1.2f;break;
            case ElementalReaction.OVERLOAD: reactionMultiplier = 2.0f;break;
        }
        int recievedDamage = (int)(reactionMultiplier * damageInfo.playerLevel * (1/*+ em bunes + %reaction bonus */));//*res mult target
        return recievedDamage;
    }
    private static int AmplifyingReaction(ElementalReaction reaction,Element triggeringElement, DamageInfo damageInfo)
    {
        float AmpMultiplier;
        if (reaction == ElementalReaction.MELT) AmpMultiplier = triggeringElement == Element.PYRO ? 2.0f : 1.5f;
        else AmpMultiplier = triggeringElement == Element.PYRO ? 1.5f : 2f;
        AmpMultiplier*= (1/*+ em bonus *//*+ reaction bonus*/);
        int recievedDamage =damageInfo.damage * (int)AmpMultiplier;
        return recievedDamage;
    }
}
