using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Settings/Elemental fonts")]
public class ElementalFontSettings : ScriptableObject
{
    public List<Color> elementFonts=new List<Color>();
    public List<Color> elementalReactionFonts = new List<Color>();

}
