using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;
using System;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{
    struct AnimState
    {
        string name;
        float time;
    }
    private string _currentAnimation;
    public Animator _anim;
    private float _animLength;
    private bool _overPlayAnimationEnded = true;
    private Coroutine _currentTimer;
    private List<AnimState> _states = new List<AnimState>();
    public AnimationDurationList animList;

#if UNITY_EDITOR
    private AnimatorController animatorController;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        animatorController = (AnimatorController)_anim.runtimeAnimatorController;
        animList.animatorController = animatorController;
        animList.RefreshList();
    }

#endif
    public void PlayAnimation(string name, bool canBePlayedOver = true)
    {
        if (_currentAnimation == name) return;
        if (!canBePlayedOver)
        {
            _overPlayAnimationEnded = false;
            _animLength = GetAnimationLength(name);
            _currentTimer = StartCoroutine(TimerCor(_animLength, SetOverPlayAnimAsEnded));
            _anim.Play(Animator.StringToHash(name)); //clipToPlay.nameHash);
            _currentAnimation = name;
        }
        if (_overPlayAnimationEnded)
        {
            _animLength = GetAnimationLength(name);
            StartCoroutine(TimerCor(_animLength, SetNormalAnimAsEneded));
            _anim.Play(Animator.StringToHash(name)); //clipToPlay.nameHash);
            _currentAnimation = name;
        }
    }
    public void PlayAnimation(string name, string layerName, bool canBePlayedOver = true)
    {
        int layer = _anim.GetLayerIndex(layerName);
        if (_currentAnimation == name) return;
        if (!canBePlayedOver)
        {
            _overPlayAnimationEnded = false;
            _animLength = GetAnimationLength(name,layer);
            _currentTimer = StartCoroutine(TimerCor(_animLength, SetOverPlayAnimAsEnded));
            _anim.Play(Animator.StringToHash(name)); //clipToPlay.nameHash);
            _currentAnimation = name;
        }
        if (_overPlayAnimationEnded)
        {
            _animLength = GetAnimationLength(name,layer);
            StartCoroutine(TimerCor(_animLength, SetNormalAnimAsEneded));
            _anim.Play(Animator.StringToHash(name)); //clipToPlay.nameHash);
            _currentAnimation = name;
        }
    }

    public void OverPlayAnimation(string name) 
    {
        string clipToPlayName = name;

        if (_currentTimer != null) StopCoroutine(_currentTimer);
        _overPlayAnimationEnded = true;

        _anim.Play(Animator.StringToHash(clipToPlayName));
        _currentAnimation = clipToPlayName;
    }

    public float GetAnimationLength(string name,int layer=0)
    {
        if (name == "Empty") return 0;
        float clipDuration = 0;
        clipDuration = animList.animations.Find(x => x.name == name && x.layer==layer).duration;
        return clipDuration;
    }
    public float GetAnimationLength(string name, string layerName)
    {
        if (name == "Empty") return 0;
        int layer = _anim.GetLayerIndex(layerName);
        float clipDuration = 0;
        clipDuration = animList.animations.Find(x => x.name == name && x.layer == layer).duration;
        return clipDuration;
    }
    public float GetAnimationSpeed(string name,int layer=0)
    {
        if (name == "Empty") return 0;
        float clipSpeed = 0;
        clipSpeed = animList.animations.Find(x => x.name == name && x.layer == layer).speed;
        return clipSpeed;
    }
    public float GetAnimationSpeed(string name, string layerName)
    {
        if (name == "Empty") return 0;
        float clipSpeed = 0;
        int layer = _anim.GetLayerIndex(layerName);
        clipSpeed = animList.animations.Find(x => x.name == name && x.layer == layer).speed;
        return clipSpeed;
    }
    private void Update()
    {

    }
    public float GetAnimationCurrentDuration(string stateName,int layer = 0)
    {
        if (_anim.GetCurrentAnimatorStateInfo(layer).IsName(stateName)) return _anim.GetCurrentAnimatorStateInfo(0).normalizedTime * _anim.GetCurrentAnimatorStateInfo(0).length;
        else return -1;
    }
    public float GetAnimationCurrentDuration(string stateName, string layerName)
    {
        int layer = _anim.GetLayerIndex(layerName);
        if (_anim.GetCurrentAnimatorStateInfo(layer).IsName(stateName)) return _anim.GetCurrentAnimatorStateInfo(layer).normalizedTime * _anim.GetCurrentAnimatorStateInfo(layer).length;
        else return -1;
    }
    public float GetCurrentAnimationRemainingLength()
    {
        return (1- _anim.GetCurrentAnimatorStateInfo(0).normalizedTime) * _animLength;
    }

    IEnumerator TimerCor(float time, Action functionToPerform)
    {
        yield return new WaitForSeconds(time);
        functionToPerform();
    }

    public void SetAnimator(bool active)
    {
        _anim.enabled = active;
    }
    public bool CheckIfAnimatorIsEnabled()
    {
        return _anim.enabled;
    }
    private void SetOverPlayAnimAsEnded()
    {
        _overPlayAnimationEnded = true;
    }
    private void SetNormalAnimAsEneded()
    {
        _currentAnimation = null;
    }
}
