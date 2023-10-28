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
                        case Element.ELECTRO:return Overload();
                        case Element.CRYO:return Melt();break;
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
                        case Element.ELECTRO:return ElectroCharged(); break;
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
                        case Element.PYRO:return Overload(); break;
                        case Element.HYDRO:return ElectroCharged(); break;
                        case Element.CRYO:return SuperConduct(); break;
                        case Element.GEO:return Crystalize(); break;
                    }
                    break;
                }
            case Element.CRYO:
                {
                    switch(damageInfo.element)
                    {
                        case Element.PYRO:return Melt(); break;
                        case Element.HYDRO:return Frozen(); break;
                        case Element.ELECTRO:return SuperConduct(); break;
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
        int ampMult = (int)(triggeringElement==Element.PYRO ? 1.5f : 2f)*(1/*+ em bonus *//*+ reaction bonus*/);
        recievedDamage = damageInfo.damage * ampMult;
        return ElementalReaction.VAPORIZE;
    }
    private static ElementalReaction Overload() { return ElementalReaction.OVERLOAD; }
    private static ElementalReaction Melt() { return ElementalReaction.MELT; }
    private static ElementalReaction ElectroCharged() { return ElementalReaction.ELECTROCHARGED; }
    private static ElementalReaction SuperConduct() { return ElementalReaction.SUPERCONDUCT; }
    private static ElementalReaction Crystalize() { return ElementalReaction.CRYSTALIZE; }

    private static ElementalReaction Frozen() { return ElementalReaction.FROZEN; }


}
