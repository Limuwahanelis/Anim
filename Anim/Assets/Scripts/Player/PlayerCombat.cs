using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] PlayerWeapon _weapon;
    [SerializeField] GameObject _weaponObject;
    [SerializeField] Transform _rightHandWeaponHold;
    [SerializeField] Transform _backWeaponHold;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCheckForEnemies(bool value)
    {
        _weapon.SetCheckForCollisions(value);
    }

    public void AttachWeaponToRightHand()
    {
        _weaponObject.transform.position = _rightHandWeaponHold.transform.position;
        _weaponObject.transform.rotation = _backWeaponHold.transform.rotation;
        _weaponObject.transform.parent = _rightHandWeaponHold;
    }
}
