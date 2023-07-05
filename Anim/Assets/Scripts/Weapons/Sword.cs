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
    private void OnTriggerEnter(Collider other)
    {
        if (!_isCheckingForCollisions) return;
        IDamagable damagable = other.GetComponent<IDamagable>();
        if(damagable!=null)
        {
            if (!_damagables.Contains(damagable))
            {
                _damagables.Add(damagable);
                damagable.TakeDamage(_dmg);
            }
        }
    }
}
