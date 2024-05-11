using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InGameMenu : MonoBehaviour
{
    [Inject]
    private PlayerInputController _controller;

    [SerializeField]
    private Settings _settings;

    private bool _isActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isActive = !_isActive;
            _settings.gameObject.SetActive(_isActive);
            if (_isActive)
            {
                _controller.SetDeactivateInteraction();
            }
            else
            {
                _controller.SetActivateInteraction();
            }
        }
    }
}
