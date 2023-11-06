using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Pool;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FloatingText : MonoBehaviour
{
    private IObjectPool<FloatingText> _pool;
    [SerializeField] TMP_Text _numberText;
    [SerializeField] Animator _anim;
    [SerializeField] LookAtConstraint _lookAtConstraint;
    [SerializeField] Vector3 _fontAnchoredPosition;
    [SerializeField] Vector2 _maxFontRandFactor;
    [SerializeField] float _duration;
    [SerializeField] float _verticalDistance;
    private float _speed;
    private float _elapsedTime;
    private void Awake()
    {
        _speed = _verticalDistance / _duration;
        ConstraintSource constraint = new ConstraintSource()
        {
            sourceTransform = Camera.main.transform,
            weight = 1
        };
        _lookAtConstraint.AddSource(constraint);
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        MoveToPostion();
        if(_elapsedTime > 0.5f) Shrink();
        
    }
    private void SetUp()
    {
        _numberText.rectTransform.localScale = new Vector3(1, 1, 1);
        _elapsedTime = 0;
    }
    public void SetText(int number,Color fontColor,Transform parent)
    {
        _numberText.text = number.ToString();
        _numberText.color = fontColor;
        _numberText.rectTransform.SetParent(parent);
        _numberText.rectTransform.localPosition = RandomizePosition(_fontAnchoredPosition);
        SetUp();
    }
    public void SetText(string text, Color fontColor, Transform parent)
    {
        _numberText.text = text;
        _numberText.color = fontColor;
        _numberText.rectTransform.SetParent(parent);
        _numberText.rectTransform.localPosition = RandomizePosition(_fontAnchoredPosition);
        SetUp();
    }
    public void SetPool(IObjectPool<FloatingText> pool)=>_pool = pool;

    public void ReturnToPool()
    {
        _pool.Release(this);
    }
    private Vector3 RandomizePosition(Vector3 position)
    {
        Vector3 toReturn = position;
        toReturn.z += Random.Range(-_maxFontRandFactor.x, _maxFontRandFactor.x);
        toReturn.y += Random.Range(-_maxFontRandFactor.y, _maxFontRandFactor.y);
        return toReturn;
    }
    private void MoveToPostion()
    {
        _numberText.rectTransform.localPosition = new Vector3(_numberText.rectTransform.localPosition.x, _numberText.rectTransform.localPosition.y+Time.deltaTime*_speed, _numberText.rectTransform.localPosition.z);
    }
    private void Shrink()
    {
        float value = Time.deltaTime * _speed*2;
        _numberText.rectTransform.localScale -= new Vector3(value, value, value);
        if (_numberText.rectTransform.localScale.x <= 0)
        {
            gameObject.SetActive(false);
            ReturnToPool();
        }
    }
}
