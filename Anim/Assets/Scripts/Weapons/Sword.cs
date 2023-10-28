using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : PlayerWeapon
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (!_isCheckingForCollisions) return;

        IDamagable damagable = other.GetComponent<IDamagable>();
        if(damagable!=null)
        {
            _damageInfo = new DamageInfo()
            {
                damage = _dmg,
                element = element,
            };
            if (!_damagables.Contains(damagable))
            {
                _damagables.Add(damagable);
                damagable.TakeDamage(_damageInfo);
            }
        }
    }


}
