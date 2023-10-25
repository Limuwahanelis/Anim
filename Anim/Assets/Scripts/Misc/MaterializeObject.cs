using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterializeObject : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    private float _emissionWidth;
    private Renderer _renderer;
    Coroutine _matCor;
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
        _matCor= StartCoroutine(MaterializeCor());
    }
    public void InstantlyMaterialize()
    {
        if(_matCor!=null)
        {
            StopCoroutine(_matCor);
            _matCor = null;
        }
        _renderer.material.SetFloat("_level", 1 + _emissionWidth);
        _renderer.material.SetFloat("_level2", -(1 + _emissionWidth));
    }
    IEnumerator MaterializeCor()
    {
        float value = 0;
        while (value < 1+_emissionWidth)
        {
            value += Time.deltaTime * speed;
            _renderer.material.SetFloat("_level", value);
            _renderer.material.SetFloat("_level2", -value);
            yield return null;
        }
        _matCor = null;
    }
}
