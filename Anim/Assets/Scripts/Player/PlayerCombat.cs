using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] PlayerWeapon _weapon;
    [SerializeField] GameObject _weaponObject;
    [SerializeField] Transform _rightHandWeaponHold;
    [SerializeField] Transform _backWeaponHold;
    [SerializeField] ComboList _comboList1;
    [SerializeField] Player _player;
    private int _comboAttackCounter=1;

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
        _weaponObject.transform.parent = _rightHandWeaponHold;
        _weaponObject.transform.localRotation = Quaternion.identity; //_backWeaponHold.transform.rotation;
    }
    public void MoveWeaponToSeath()
    {
        _weaponObject.transform.position = _backWeaponHold.transform.position;
        _weaponObject.transform.parent = _backWeaponHold;
        _weaponObject.transform.localRotation = Quaternion.identity; //_backWeaponHold.transform.rotation;
    }
    public void ResetComboCounter()
    {
        _comboAttackCounter = 1;
    }
    public void PerformNextAttackInCombo(in PlayerAttackingState attackState)
    {
        if (_comboAttackCounter > _comboList1.comboList.Count)
        {
            return;
        }
        if(_player.animManager.GetAnimationCurrentDuration("Attack "+_comboAttackCounter) >= _comboList1.comboList[_comboAttackCounter-1].nextAttackWindowStart / _player.animManager.GetAnimationSpeed("Attack " + _comboAttackCounter) && _player.animManager.GetAnimationCurrentDuration("Attack " + _comboAttackCounter)  <= _comboList1.comboList[_comboAttackCounter - 1].nextAttackWindowEnd / _player.animManager.GetAnimationSpeed("Attack " + _comboAttackCounter))
        {
            _weapon.ResetTargets();
            attackState.ResetTimer();
            _player.anim.SetTrigger("Attack");
            _comboAttackCounter++;
            attackState.StartWaitingForAttackEnd("Attack " + _comboAttackCounter);
        }
    }
}
