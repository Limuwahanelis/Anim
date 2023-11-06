using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FloatingTextPool : MonoBehaviour
{
    [SerializeField] FloatingText _damageNumberPrefab;
    [SerializeField] ObjectPool<FloatingText> _numbersPool;
    // Start is called before the first frame update
    void Start()
    {
        _numbersPool = new ObjectPool<FloatingText>(CreateText, OnTakeTextFromPool, OnReturnTextToPool);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public FloatingText GetText()
    {
        return _numbersPool.Get();
    }
    FloatingText CreateText()
    {
        FloatingText damageNumber = Instantiate(_damageNumberPrefab);
        damageNumber.SetPool(_numbersPool);
        return damageNumber;
    }
    public void OnTakeTextFromPool(FloatingText damageNumber)
    {
        damageNumber.gameObject.SetActive(true);
    }
    public void OnReturnTextToPool(FloatingText damageNumber)
    {
        damageNumber.gameObject.SetActive(false);
    }
}
