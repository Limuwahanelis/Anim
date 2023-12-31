using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] Transform _objectToFollow;
    private Vector3 _offset;
    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - _objectToFollow.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _objectToFollow.position + _offset;
    }
}
