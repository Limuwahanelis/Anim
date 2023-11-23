using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachFromObjectOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        if(transform.GetComponent<Canvas>())
        {
            Vector3 orgScale = transform.localScale;
            transform.localScale = Vector3.one;
            transform.SetParent(null);
            transform.localScale = orgScale;
        }
        else transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
