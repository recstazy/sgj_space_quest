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
    private Color _color;

    private Material _material;
    private Color _regularColor;

    private void Start()
    {
        _material = _meshRenderer.materials[_materialIndex];
    }

    public void ToggleOn()
    {
        _material.color = _color;
    }

    public void ToggleOff()
    {
        _material.color = _regularColor;
    }

}
