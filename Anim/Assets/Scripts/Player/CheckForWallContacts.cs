using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForWallContacts : MonoBehaviour
{
    public Action OnNearWall;
    public Action OnLeftWall;
    public List<Collider> Walls=>_walls;

    private List<Collider> _walls = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIt");
        if(_walls.Count == 0)  OnNearWall?.Invoke();
        if(!_walls.Contains(other))  _walls.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_walls.Contains(other)) _walls.Remove(other);
        if (_walls.Count == 0) OnLeftWall?.Invoke();
    }

}
