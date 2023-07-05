using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class FloatReference
{
    public bool useConstant = true;
    public float constantValue;
    public FloatValue variable;

    public float value
    {
        get { return useConstant ? constantValue : variable.value; }
        set 
        {
            if (useConstant) constantValue = value;
            else variable.value = value;
        }
    }
}
