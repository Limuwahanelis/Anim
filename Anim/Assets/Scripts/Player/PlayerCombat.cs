using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] GameObject _weapon;
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

    public void AttachWeaponToRightHand()
    {
        _weapon.transform.position = _rightHandWeaponHold.transform.position;
        _weapon.transform.rotation = _backWeaponHold.transform.rotation;
        _weapon.transform.parent = _rightHandWeaponHold;
    }
}
