using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ComboAttack")]
public class ComboAttack : ScriptableObject
{
    public float nextAttackWindowStart => _nextAttackWindowStart;
    public float nextAttackWindowEnd => _nextAttackWindowEnd;
    [SerializeField] float _nextAttackWindowStart;
    [SerializeField] float _nextAttackWindowEnd;
    [SerializeField] AnimationClip _associatedAnimation;
}