using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationData
{
    public string name;
    public float duration;

    public AnimationData(string name, float duration)
    {
        this.name = name;
        this.duration = duration;
    }
}
