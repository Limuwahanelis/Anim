using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Pool;
using UnityEngine.UI;
public class DamageNumber : MonoBehaviour
{
    private IObjectPool<DamageNumber> _pool;
    [SerializeField] TMP_Text _numberText;
    [SerializeField] Animator _anim;
    [SerializeField] LookAtConstraint _lookAtConstraint;
    [SerializeField] Vector3 _fontAnchoredPosition;

    private void Awake()
    {
        ConstraintSource constraint = new ConstraintSource()
        {
            sourceTransform = Camera.main.transform,
            weight = 1
        };
        _lookAtConstraint.AddSource(constraint);
    }
    public void SetNumber(int number,Color fontColor,Transform parent)
    {
        _numberText.text = number.ToString();
        _numberText.color = fontColor;
        _numberText.rectTransform.SetParent(parent);
        _numberText.rectTransform.localPosition = _fontAnchoredPosition;
    }
    
    public void SetPool(IObjectPool<DamageNumber> pool)=>_pool = pool;

    public void ReturnToPool()
    {
        _pool.Release(this);
    }
}
