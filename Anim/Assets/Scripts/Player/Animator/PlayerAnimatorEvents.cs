using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimatorEvents : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent OnWeaponSwitch = new UnityEvent();

    public void WeaponSwitch()=>OnWeaponSwitch?.Invoke();
}
