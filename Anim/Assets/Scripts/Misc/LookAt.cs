using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform _objectToLookAt;
    // Update is called once per frame
    void Update()
    {
        transform.forward = _objectToLookAt.forward;
    }
}
