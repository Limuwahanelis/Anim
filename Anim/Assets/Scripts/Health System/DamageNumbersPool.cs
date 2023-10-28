using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamageNumbersPool : MonoBehaviour
{
    [SerializeField] DamageNumber _damageNumberPrefab;
    [SerializeField] ObjectPool<DamageNumber> _numbersPool;
    // Start is called before the first frame update
    void Start()
    {
        _numbersPool = new ObjectPool<DamageNumber>(CreateDamageNumber, OnTakeNumberFromPool, OnReturnNumberToPool);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public DamageNumber GetDamageNumber()
    {
        return _numbersPool.Get();
    }
    DamageNumber CreateDamageNumber()
    {
        DamageNumber damageNumber = Instantiate(_damageNumberPrefab);
        damageNumber.SetPool(_numbersPool);
        return damageNumber;
    }
    public void OnTakeNumberFromPool(DamageNumber damageNumber)
    {
        damageNumber.gameObject.SetActive(true);
    }
    public void OnReturnNumberToPool(DamageNumber damageNumber)
    {
        damageNumber.gameObject.SetActive(false);
    }
}
