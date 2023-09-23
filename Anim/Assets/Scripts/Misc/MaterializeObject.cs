using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterializeObject : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    float _value = 0f;
    private float _emissionWidth;
    private Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _emissionWidth = _renderer.material.GetFloat("_emission_width");

    }

    // Update is called once per frame
    void Update()
    {
         
    }
    public void Dematerialize()
    {
        _renderer.material.SetFloat("_level", -_emissionWidth);
        _renderer.material.SetFloat("_level2", _emissionWidth);
    }
    public void Materialize()
    {
        StartCoroutine(MaterializeCor());
    }
    IEnumerator MaterializeCor()
    {
        while (_value < 1+_emissionWidth)
        {
            _value += Time.deltaTime * speed;
            _renderer.material.SetFloat("_level", _value);
            _renderer.material.SetFloat("_level2", -_value);
            yield return null;
        }
        _value = 0f;
    }
}
