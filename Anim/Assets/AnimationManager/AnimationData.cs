using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationData
{
    public string name;
    public float duration;
    public float speed;

    public AnimationData(string name, float duration, float speed)
    {
        this.name = name;
        this.duration = duration;
        this.speed = speed;
    }
}
