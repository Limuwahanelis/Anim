using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public float CurrentStamina=>_currentStamina;

    [SerializeField] Image _fillImage;
    [SerializeField] Image _backgroundImage;
    [SerializeField] float _maxStamina;
    [SerializeField] float _currentStamina;
    [SerializeField] float _staminaRegenRate;
    private bool _isRegenerating;
    Coroutine _regenCor;
    // Start is called before the first frame update
    void Start()
    {
        _fillImage.fillAmount = _maxStamina;
        _backgroundImage.fillAmount = _maxStamina;
    }

    private void OnValidate()
    {
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
        _fillImage.fillAmount = _currentStamina;
        _backgroundImage.fillAmount = _maxStamina;
    }

    public void SetCurrentStamina(float pctValue)
    {
        
        _currentStamina = _maxStamina * (pctValue / 100f);
        _currentStamina = math.clamp(_currentStamina,0,_maxStamina);
        _fillImage.fillAmount = _currentStamina;
    }
    public void IncreaseCurrentStamina(float pctValue)
    {
        _currentStamina+= _maxStamina * (pctValue / 100f);
        _currentStamina = math.clamp(_currentStamina, 0, _maxStamina);
        _fillImage.fillAmount = _currentStamina;
    }

    public void StartRegeneratingStamina()
    {
       _regenCor = StartCoroutine(StaminaRegenerateCor());
    }
    public void StopRegenerating()
    {
        if (_regenCor != null) StopCoroutine(_regenCor);
        _regenCor = null;
    }
    IEnumerator StaminaRegenerateCor()
    {
        if (_isRegenerating)
        {
            _regenCor = null;
            yield break;
        }
        _isRegenerating = true;
        while (_currentStamina < _maxStamina)
        {
            IncreaseCurrentStamina(_staminaRegenRate * Time.deltaTime);
            yield return null;
        }
        _isRegenerating=false;
    }
}
