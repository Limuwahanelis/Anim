using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ReactionCalculator;

public class ElementalStatus : MonoBehaviour
{
    [SerializeField] SpriteRenderer _elementIndicator;
    [SerializeField] ElementalFontSettings _elementFontSettings;
    public ReactionCalculator.Element afflictedElement;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ElementalReaction SetElement(DamageInfo damageinfo,out int recievedDamage,out Color damageFontColor,out Color elementalReactionColor)
    {
        ReactionCalculator.ElementalReaction reaction = ReactionCalculator.CalculateDamage(afflictedElement, damageinfo, out recievedDamage);
        damageFontColor = _elementFontSettings.elementFonts[((int)damageinfo.element)];
        Debug.Log(reaction);
        if (reaction == ReactionCalculator.ElementalReaction.NONE)
        {
            afflictedElement = damageinfo.element;
            elementalReactionColor = _elementFontSettings.elementalReactionFonts[((int)damageinfo.element)];
            _elementIndicator.color = damageFontColor;
            _elementIndicator.gameObject.SetActive(true);
        }
        else
        {
            afflictedElement = ReactionCalculator.Element.PHYSICAL;
            _elementIndicator.gameObject.SetActive(false);
            elementalReactionColor = _elementFontSettings.elementalReactionFonts[((int)reaction)];
        }
        return reaction;
        
    }
    public void RemoveElement()
    {
        afflictedElement = ReactionCalculator.Element.PHYSICAL;
        _elementIndicator.color = _elementFontSettings.elementFonts[((int)ReactionCalculator.Element.PHYSICAL)];
    }
    private void OnDestroy()
    {

    }
}
