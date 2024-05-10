using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Recstazy.SiberianFootsteps.Demo
{
    public class FootstepSurface : MonoBehaviour
    {
        [SerializeField]
        private Color _color = Color.white;

        [SerializeField]
        private TextMesh _text;

        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        private Collider _collider;

        private void Awake()
        {
            _renderer.material.color = _color;
            _text.text = _collider.sharedMaterial == null ? "No Material" : _collider.sharedMaterial.name;
        }
    }
}
