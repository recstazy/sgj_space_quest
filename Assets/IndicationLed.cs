using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicationLed : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private int _materialIndex;
    [SerializeField]
    private Color _enabledcolor;
    [SerializeField]
    private Color _enabledEmissioncolor;

    private Material _material;
    private Color _disabledColor;
    private Color _disabledEmissionColor;

    private void Start()
    {
        _material = _meshRenderer.materials[_materialIndex];
        _disabledColor = _material.color;
        _disabledEmissionColor = _material.GetColor("_EmissionColor");
    }

    public void ToggleOn()
    {
        _material.color = _enabledcolor;
        _material.SetColor("_EmissionColor", _enabledEmissioncolor);
    }

    public void ToggleOff()
    {
        _material.color = _disabledColor;
        _material.SetColor("_EmissionColor", _disabledEmissionColor);
    }

}
