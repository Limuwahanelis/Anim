using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BoolReference
{
    public bool useConstant = true;
    public bool constantValue;
    public BoolValue variable;

    public bool value
    {
        get { return useConstant ? constantValue : variable.value; }
        set
        {
            if (useConstant) constantValue = value;
            else variable.value = value;
        }
    }
}
