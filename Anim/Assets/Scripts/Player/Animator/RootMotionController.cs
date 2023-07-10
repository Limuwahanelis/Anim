using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    public string[] animationTag;
    [SerializeField] Animator anim;
    [SerializeField] GameObject playerGO;

    private void Awake()
    {
    }
    private void OnAnimatorMove()
    {


        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        
        for (int i = 0; i < animationTag.Length; i++)
        {
            if (stateInfo.IsTag(animationTag[i]))
            {
                playerGO.transform.position += anim.deltaPosition;
                playerGO.transform.rotation *= anim.deltaRotation;
                anim.ApplyBuiltinRootMotion();

            }
            else anim.applyRootMotion = false;
        }
    }
}
