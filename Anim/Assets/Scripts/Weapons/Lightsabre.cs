using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsabre : MonoBehaviour
{
    [SerializeField] GameObject _mainBlade;
    [SerializeField] GameObject _bladeTip;
    [SerializeField] float _length;
    [SerializeField] float _thickness;
    [SerializeField] Transform _handlePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnValidate()
    {
        _mainBlade.transform.localScale = new Vector3(_thickness, _length, _thickness);
        _mainBlade.transform.localPosition = new Vector3(_handlePos.transform.localPosition.x, _handlePos.localPosition.y + _length+_handlePos.localScale.y);
        _bladeTip.transform.localPosition = new Vector3(_handlePos.localPosition.x, _mainBlade.transform.localPosition.y + _length, _handlePos.localPosition.z);
        _bladeTip.transform.localScale=new Vector3(_thickness, _thickness, _thickness);
    }
}
